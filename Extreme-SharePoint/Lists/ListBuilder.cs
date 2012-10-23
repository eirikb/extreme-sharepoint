using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme.Lists
{
    internal class Field
    {
        public bool AllowMulti;
        public bool Hidden;
        public bool Required;
        public string Title;
        public SPFieldType Type;
    }

    internal class LookupField : Field
    {
        public Guid LookupList;
    }

    public static class ListBuilder
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static void EnsureLists(SPWeb web)
        {
            EnsureTeamsList(web);
            EnsureStatsList(web);
        }

        public static SPList EnsureTeamsList(SPWeb web)
        {
            var list = web.Lists.TryGetList("Teams");
            if (list != null) return list;
            list = BuildList(web, "Teams", new List<Field>
                {
                    new Field {Title = "Score", Type = SPFieldType.Number, Required = false},
                    new Field {Title = "Players", Type = SPFieldType.User, Required = false, AllowMulti = true},
                    new Field {Title = "Host", Type = SPFieldType.Text, Required = true, Hidden = true}
                });

            var titleField = list.Fields.GetFieldByInternalName("Title");
            titleField.Title = "Team name";
            titleField.Update();

            var scoreField = list.Fields.GetFieldByInternalName("Score");
            scoreField.ShowInNewForm = false;
            scoreField.ShowInEditForm = false;
            scoreField.Update();

            var view = list.DefaultView;
            view.ViewFields.Add("Score");
            view.Update();

            // Create and Edit access: Create items and edit items that were created by the user 
            list.WriteSecurity = 2;
            list.Update();

            return list;
        }

        public static SPList EnsureStatsList(SPWeb web)
        {
            var list = web.Lists.TryGetList("Stats");
            if (list != null) return list;
            var teamsList = web.Lists["Teams"];
            var fields = new List<Field>
                {
                    new LookupField {Title = "Team", LookupList = teamsList.ID, Required = true, Hidden = true},
                    new Field {Title = "Time", Type = SPFieldType.DateTime, Required = true},
                    new Field {Title = "Question", Type = SPFieldType.Text, Required = false, Hidden = true},
                    new Field {Title = "Answer", Type = SPFieldType.Text, Required = false},
                    new Field {Title = "Points", Type = SPFieldType.Number, Required = false},
                    new Field {Title = "Level", Type = SPFieldType.Number, Required = false, Hidden = true},
                };
            list = BuildList(web, "Stats", fields);
            // Read items that were created by the user
            list.ReadSecurity = 2;
            // Create and Edit access: None
            list.WriteSecurity = 4;
            list.Update();

            var view = list.DefaultView;
            view.ViewFields.Delete("LinkTitle");
            view.Update();

            var titleField = list.Fields.GetFieldByInternalName("LinkTitle");
            titleField.Hidden = true;
            titleField.ShowInViewForms = false;
            titleField.Update();

            var viewFields = new StringCollection();
            viewFields.AddRange(fields.Select(field => field.Title).ToArray());
            list.Views.Add("Godmode", viewFields, null, 100, true, false, SPViewCollection.SPViewType.Html, true);
            return list;
        }

        private static SPList BuildList(SPWeb web, string title, List<Field> fields)
        {
            Log.InfoFormat("  List does not exist: {0}, creating...", title);
            web.Lists.Add(title, "", SPListTemplateType.GenericList);
            var list = web.Lists[title];
            var view = list.DefaultView;
            fields.ForEach(field =>
                {
                    if (field is LookupField)
                    {
                        var lookupField = field as LookupField;
                        list.Fields.AddLookup(field.Title, lookupField.LookupList, field.Required);
                    }
                    else list.Fields.Add(field.Title, field.Type, field.Required);

                    var spField = list.Fields.GetFieldByInternalName(field.Title);
                    if (field.Hidden)
                    {
                        spField.ShowInDisplayForm = false;
                        spField.ShowInViewForms = false;
                        spField.Update();
                        spField = list.Fields.GetFieldByInternalName(field.Title);
                    }
                    else view.ViewFields.Add(spField);

                    if (!field.AllowMulti) return;
                    var spLookupField = spField as SPFieldLookup;
                    if (spLookupField == null) return;
                    spLookupField.AllowMultipleValues = field.AllowMulti;
                    spLookupField.Update();
                });
            view.Update();
            return list;
        }
    }
}