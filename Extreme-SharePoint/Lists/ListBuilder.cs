using System;
using System.Collections.Generic;
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

            list.WriteSecurity = 2;
            list.Update();

            return list;
        }

        public static SPList EnsureStatsList(SPWeb web)
        {
            var list = web.Lists.TryGetList("Stats");
            if (list != null) return list;
            var teamsList = web.Lists["Teams"];
            list = BuildList(web, "Stats", new List<Field>
                {
                    new LookupField{Title = "Team", LookupList = teamsList.ID, Required = true, Hidden = true},
                    new Field {Title = "Success", Type = SPFieldType.Boolean, Required = true},
                    new Field {Title = "Time", Type = SPFieldType.DateTime, Required = true},
                    new Field {Title = "Question", Type = SPFieldType.Text, Required = false, Hidden = true},
                    new Field {Title = "Answer", Type = SPFieldType.Text, Required = false, Hidden = true},
                    new Field {Title = "Points", Type = SPFieldType.Number, Required = false, Hidden = true},
                    new Field {Title = "Level", Type = SPFieldType.Number, Required = false, Hidden = true},
                });
            list.ReadSecurity = 2;
            list.WriteSecurity = 4;
            list.Update();

            var view = list.DefaultView;
            view.ViewFields.Delete("LinkTitle");
            view.Update();

            var titleField = list.Fields.GetFieldByInternalName("LinkTitle");
            titleField.Hidden = true;
            titleField.ShowInViewForms = false;
            titleField.Update();

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
                    } else view.ViewFields.Add(spField);

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