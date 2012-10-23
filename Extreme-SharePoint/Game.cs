using System;
using System.Net;
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
            Level = 1;
            _web = web;
            Run = true;
        }

        public bool Run { get; set; }

        public int Level { get; set; }

        public void Ping()
        {
            Log.Debug("Game pinged");

            var question = Question.GetRandomQuestion(Level);
            if (question == null)
            {
                Log.Debug("No question found");
                return;
            }
            Log.Debug("Quering clients...");

            ListsQuery.GetTeams(_web).ForEach(team => Client.Request(team, question, (sender, args) =>
                {
                    var teamId = team.ID;
                    using (var site = new SPSite(_web.Url))
                    {
                        using (var web = site.OpenWeb(_web.ServerRelativeUrl))
                        {
                            try
                            {
                                OnClientResponse(web, teamId, question, args);
                            }
                            catch (Exception e)
                            {
                                Log.Error("Client.Request crash!");
                                Log.Error(e);
                            }
                        }
                    }
                }));
        }

        private static void OnClientResponse(SPWeb web, int teamId, IQuestion question,
                                             DownloadStringCompletedEventArgs args)
        {
            var team = web.Lists["Teams"].GetItemById(teamId);
            int points;
            string result;
            if (args.Error != null || args.Cancelled)
            {
                if (args.Error != null)
                    Log.InfoFormat("Request failed for team {0}! {1} - {2}", team.Title,
                                   args.Error.Message, args.Error.InnerException);
                else Log.InfoFormat("Request cancelled for team {0}", team.Title);
                points = -question.Level;
                result = "Server error";
            }
            else
            {
                result = args.Result;
                Log.DebugFormat("Got result {0}", result);
                if (string.IsNullOrEmpty(result)) points = -(question.Level/2);
                else points = question.Run(result) ? question.Level : -(question.Level/2);
                Log.DebugFormat("Points : {0}", points);
            }
            var stats = web.Lists["Stats"];
            var statsItem = stats.AddItem();
            statsItem["Team"] = team;
            statsItem["Author"] = team["Author"];
            statsItem["Time"] = DateTime.Now;
            statsItem["Question"] = question.Question;
            statsItem["Answer"] = result;
            statsItem["Points"] = points;
            statsItem["Level"] = question.Level;
            statsItem.Update();

            int currentScore;
            if (!int.TryParse("" + team["Score"], out currentScore))
            {
                Log.WarnFormat("Unable to format {0} to int for teamscore of team {1}",
                               team["Score"],
                               team.Title);
                currentScore = 0;
            }
            currentScore += points;
            team["Score"] = currentScore;
            team.Update();
        }
    }
}