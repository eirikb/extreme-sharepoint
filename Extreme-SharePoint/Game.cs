using System;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    internal class Game
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

            Lists.Lists.GetTeamsWithPlayer(_web).ForEach(team =>
                {
                    var players = team["Players"] as SPFieldUserValueCollection;
                    if (players == null) return;
                    var player = players.First();

                    var teamScore = Lists.Lists.GetTeamScore(_web, team);
                    var question = Question.GetRandomQuestion(Level);
                    var host = team["Host"];

                    using (var client = new WebClient())
                    {
                        var url = new Uri(new Uri("" + host),question.Question);
                        Log.DebugFormat("Sending request to {0}", url);
                        var result = client.DownloadString(url);
                        Log.DebugFormat("Got result: {0}", result);
                        var points = question.Run(result);
                        Log.DebugFormat("Points : {0}", points);
                        var statsItem = stats.AddItem();
                        statsItem["Success"] = points > 0;
                        statsItem["Author"] = player;
                        statsItem["Time"] = DateTime.Now;
                        statsItem.Update();

                        int currentScore;
                        if (!int.TryParse("" + teamScore["Score"], out currentScore))
                        {
                            Log.WarnFormat("Unable to format {0} to int for teamscore of team {1}", teamScore["Score"],
                                           team.Title);
                            return;
                        }
                        currentScore += points;
                        teamScore["Score"] = currentScore;
                        teamScore.Update();
                    }
                });
        }
    }
}