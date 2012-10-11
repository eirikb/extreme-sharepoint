using System;
using Eirikb.SharePoint.Extreme.Lists;
using ManyConsole;
using Microsoft.SharePoint;
using System.Linq;
using log4net;

namespace Eirikb.SharePoint.Extreme.Commands
{
    public class Ls : ConsoleCommand
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public Ls()
        {
            IsCommand("ls", "List all teams");
        }

        public override int Run(string[] remainingArguments)
        {
            var web = ExtremeSharePoint.Web;
            if (web == null)
            {
                Log.Warn("SPWeb in ExtremeSharpoint is not set");
                return 0;
            }
            var teams = ListsQuery.GetTeams(web);
            teams.Sort((a, b) =>
                {
                    int sa;
                    if (!int.TryParse("" + a["Score"], out sa)) return 0;
                    int sb;
                    if (!int.TryParse("" + b["Score"], out sb)) return 0;
                    return sb - sa;
                });
            teams.ForEach(team =>
                {
                    var players = team["Players"] as SPFieldUserValueCollection;
                    var ps = players != null ? string.Join(", ", players.Select(u => u.User.Name).ToArray()) : "";
                    Console.WriteLine("{0} - {1} - {2} - {3}", team.Title, team["Score"], ps, team["Author"]);
                });
            return 0;
        }
    }
}