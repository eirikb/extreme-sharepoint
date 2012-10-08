using System;
using System.Linq;
using ManyConsole;
using log4net;
using log4net.Core;

namespace Eirikb.SharePoint.Extreme.Commands
{
    internal class LogLevel : ConsoleCommand
    {
        public LogLevel()
        {
            IsCommand("log", "set log level");
            HasAdditionalArguments(1, "log level, such as info and debug");
        }

        public override int Run(string[] remainingArguments)
        {
            if (remainingArguments.Length < 1)
            {
                Console.WriteLine("Please provide a log level");
                return 0;
            }

            try
            {
                var level =
                    LogManager.GetRepository().LevelMap.AllLevels.Cast<Level>().FirstOrDefault(
                        l => l.Name.ToLower() == remainingArguments[0]);
                if (level == null)
                {
                    Console.WriteLine("No such level: {0}", remainingArguments[0]);
                    return 0;
                }
                LogManager.GetRepository().Threshold = level;
                Console.WriteLine("Log level set to {0}", level.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to cast {0} to log level. Error: {1}", remainingArguments[0], e.Message);
            }
            return 0;
        }
    }
}