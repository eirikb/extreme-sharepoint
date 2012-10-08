using System;
using ManyConsole;
using log4net;

namespace Eirikb.SharePoint.Extreme.Commands
{
    public class QuestionLevel: ConsoleCommand
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public QuestionLevel()
        {
            IsCommand("level", "Set the question level");
            HasAdditionalArguments(1, "Question level");
        }

        public override int Run(string[] remainingArguments)
        {
            if (remainingArguments.Length < 1)
            {
                Console.WriteLine("Please provide a log level");
                return 0;
            }
            var game = ExtremeSharePoint.Game;
            if (game == null)
            {
                Log.Warn("Game not set");
                return 0;
            }

            int level;
            if (!int.TryParse(remainingArguments[0], out level))
            {
                Log.WarnFormat("Unable to parse level {0} to int", remainingArguments[0]);
                return 0;
            }
            Log.InfoFormat("Setting question level to {0}", level);
            game.Level = level;

            return 0;
        }
    }
}