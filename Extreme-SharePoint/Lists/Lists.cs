using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using NecroNet.SharePoint.CodeCaml;
using log4net;

namespace Eirikb.SharePoint.Extreme.Lists
{
    public static class Lists
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static List<SPListItem> GetTeams(SPWeb web)
        {
            return web.Lists["Teams"].GetItems(new SPQuery()).Cast<SPListItem>().ToList();
        }

        public static List<SPListItem> GetTeamsWithPlayer(SPWeb web)
        {
            return GetTeams(web).Where(team =>
                {
                    var players = team["Players"] as SPFieldUserValueCollection;
                    if (players == null || players.Count == 0)
                    {
                        Log.WarnFormat("Players not found for {0}", team.Title);
                        return false;
                    }
                    return true;
                }).ToList();
        }

        public static SPListItem GetTeamScore(SPWeb web, SPListItem team)
        {
            var score = web.Lists["Score"];
            var query = new SPQuery
                {Query = CQ.Where(CQ.Eq.FieldRef(CQ.FieldRef("Team").LookupId(true)).Value(team.ID))};

            var teamScore = score.GetItems(query).Cast<SPListItem>().FirstOrDefault();
            if (teamScore != null) return teamScore;
            teamScore = score.AddItem();
            teamScore["Title"] = team.Title;
            teamScore["Score"] = 0;
            teamScore["Team"] = new SPFieldLookupValue(team.ID, team.Title);
            teamScore.Update();
            return teamScore;
        }
    }
}