using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme.Lists
{
    internal class Field
    {
        public bool Required;
        public string Title;
        public SPFieldType Type;
        public bool AllowMulti;
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
            EnsureScoreList(web);
            EnsureStatsList(web);
        }

        public static SPList EnsureTeamsList(SPWeb web)
        {
            var list = GetOrBuildList(web, "Teams", new List<Field>
                {
                    new Field {Title = "Players", Type = SPFieldType.User, Required = true, AllowMulti = true},
                    new Field {Title = "Host", Type = SPFieldType.URL, Required = true}
                });

            var titleField = list.Fields.GetFieldByInternalName("Title");
            titleField.Title = "Team name";
            titleField.Update();

            return list;
        }

        public static SPList EnsureScoreList(SPWeb web)
        {
            var teamList = web.Lists["Teams"];

            var list = GetOrBuildList(web, "Score", new List<Field>
                {
                    new Field {Title = "Score", Type = SPFieldType.Number, Required = true},
                    new LookupField {Title = "Team", LookupList = teamList.ID},
                });

            var titleField = list.Fields.GetFieldByInternalName("Title");
            titleField.Title = "Team name";
            titleField.Update();

            return list;
        }

        public static SPList EnsureStatsList(SPWeb web)
        {
            var list = GetOrBuildList(web, "Stats", new List<Field>
                {
                    new Field {Title = "Success", Type = SPFieldType.Boolean, Required = true},
                    new Field {Title = "Time", Type = SPFieldType.DateTime, Required = true},
                });
            list.ReadSecurity = 2;
            list.WriteSecurity = 2;
            list.Update();
            return list;
        }

        private static SPList GetOrBuildList(SPWeb web, string title, List<Field> fields)
        {
            var list = web.Lists.TryGetList(title);
            if (list == null)
            {
                Log.InfoFormat("  List does not exist: {0}, creating...", title);
                web.Lists.Add(title, "", SPListTemplateType.GenericList);
                list = web.Lists[title];
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
                        view.ViewFields.Add(spField);

                        if (!field.AllowMulti) return;
                        var spLookupField = spField as SPFieldLookup;
                        if (spLookupField == null) return;
                        spLookupField.AllowMultipleValues = field.AllowMulti;
                        spLookupField.Update();
                    });
                view.Update();
            }
            else Log.InfoFormat("  List exists: {0}", title);

            return list;
        }
    }
}