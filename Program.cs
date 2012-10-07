using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ManyConsole;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        private static void Main()
        {
            Log.Info("Connecting to SharePoint...");
            using (var site = new SPSite("http://localhost"))
            {
                using (var web = site.OpenWeb())
                {
                    Log.Info("Ensuring lists...");
                    ListBuilder.EnsureTeamList(web);
                    ListBuilder.EnsureScoreList(web);
                    ListBuilder.EnsureStatsList(web);

                    Log.Info("Let the game begin!");

                    var game = new Game();

                    new Thread(() =>
                        {
                            while (true)
                            {
                                lock (game)
                                {
                                    if (!game.Run) break;
                                    game.Ping();
                                    Monitor.Wait(game, TimeSpan.FromSeconds(5));
                                    if (!game.Run) break;
                                }
                            }
                        }).Start();

                    var commands = GetCommands();
                    var consoleRunner = new ConsoleModeCommand(GetCommands);
                    commands = commands.Concat(new[] {consoleRunner});

                    while (true)
                    {
                        var cmd = new[] {Console.ReadLine()};
                        var result = ConsoleCommandDispatcher.DispatchCommand(commands, cmd, Console.Out);
                        if (result < -1) break;
                    }
                    lock (game)
                    {
                        game.Run = false;
                        Monitor.Pulse(game);
                    }
                }
            }
        }

        private static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (Program));
        }
    }
}