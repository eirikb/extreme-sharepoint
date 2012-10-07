using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    internal static class ListBuilder
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static SPList EnsureTeamList(SPWeb web)
        {
            var list = GetOrBuildList(web, "Team");
            list.Fields.Add("Players", SPFieldType.User, true);
            list.Fields.Add("Host", SPFieldType.Text, true);

            var titleField = list.Fields.GetFieldByInternalName("Title");
            titleField.Title = "Team name";
            titleField.Update();

            return list;
        }

        public static SPList EnsureScoreList(SPWeb web)
        {
            var teamList = web.Lists["Team"];

            var list = GetOrBuildList(web, "Score");
            list.Fields.Add("Score", SPFieldType.Number, true);
            list.Fields.AddLookup("Team", teamList.ID, true);

            var titleField = list.Fields.GetFieldByInternalName("Title");
            titleField.Title = "Team name";
            titleField.Update();

            return list;
        }

        public static SPList EnsureStatsList(SPWeb web)
        {
            var list = GetOrBuildList(web, "Stats");
            list.Fields.Add("Success", SPFieldType.Boolean, true);
            return list;
        }

        private static SPList GetOrBuildList(SPWeb web, string title)
        {
            var list = web.Lists.TryGetList(title);
            if (list == null)
            {
                Log.InfoFormat("  List does not exist: {0}, creating...", title);
                web.Lists.Add(title, "", SPListTemplateType.GenericList);
                list = web.Lists[title];
            }
            else Log.InfoFormat("  List exists: {0}", title);

            return list;
        }
    }
}