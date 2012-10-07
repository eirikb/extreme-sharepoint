using System;
using ManyConsole;

namespace Eirikb.SharePoint.Extreme.commands
{
        public class GetTime : ConsoleCommand
        {
            public GetTime()
            {
                IsCommand("get-time", "Returns the current system time.");
            }

            public override int Run(string[] remainingArguments)
            {
                Console.WriteLine(DateTime.UtcNow);

                return 0;
            }
        }
}
