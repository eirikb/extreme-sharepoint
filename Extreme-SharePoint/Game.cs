using System;
using System.Linq;
using Eirikb.SharePoint.Extreme.Lists;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    public class Game
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");
        private readonly SPWeb _web;

        public Game(SPWeb web)
        {
            Run = true;
            Level = 1;
            _web = web;
        }

        public int Level { get; set; }

        public bool Run { get; set; }

        public void Ping()
        {
            Log.Debug("Game pinged");

            var stats = _web.Lists["Stats"];

            ListsQuery.GetTeamsWithPlayer(_web).ForEach(team =>
                {
                    var players = team["Players"] as SPFieldUserValueCollection;
                    if (players == null) return;
                    var player = players.First();

                    var teamScore = ListsQuery.GetTeamScore(_web, team);
                    var question = Question.GetRandomQuestion(Level);
                    if (question == null)
                    {
                        Log.Debug("No question found");
                        return;
                    }
                    var result = Client.Request(team, question);
                    var points = string.IsNullOrEmpty(result) ? -2 : question.Run(result);
                    Log.DebugFormat("Points : {0}", points);
                    var statsItem = stats.AddItem();
                    statsItem["Success"] = points > 0;
                    statsItem["Author"] = player;
                    statsItem["Time"] = DateTime.Now;
                    statsItem.Update();

                    int currentScore;
                    if (!int.TryParse("" + teamScore["Score"], out currentScore))
                    {
                        Log.WarnFormat("Unable to format {0} to int for teamscore of team {1}",
                                       teamScore["Score"],
                                       team.Title);
                        return;
                    }
                    currentScore += points;
                    teamScore["Score"] = currentScore;
                    teamScore.Update();
                });
        }
    }
}