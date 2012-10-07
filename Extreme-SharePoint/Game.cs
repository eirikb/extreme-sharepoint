using System;
using Eirikb.SharePoint.Extreme.questions;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    internal class Game
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public Game()
        {
            Run = true;
            Level = 1;
        }

        public int Level { get; set; }

        public bool Run { get; set; }

        public void Ping()
        {
            Log.Info("Game pinged");
            Console.WriteLine("OMG!");

            var question = Question.GetRandomQuestion(Level);
            Console.WriteLine("Hei: " + question.Run("Hei"));
            Console.WriteLine("Hack: " + question.Run("Hack"));
        }
    }
}