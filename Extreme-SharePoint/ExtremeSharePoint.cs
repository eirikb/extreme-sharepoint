using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Eirikb.SharePoint.Extreme.Lists;
using ManyConsole;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    public static class ExtremeSharePoint
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static string URL = "http://localhost";

        public static void Start(Type start)
        {
            var confurl = ConfigurationManager.AppSettings["url"];
            if (!string.IsNullOrEmpty(confurl)) URL = confurl;

            Log.Info("Connecting to SharePoint...");
            using (var site = new SPSite(URL))
            {
                using (var web = site.OpenWeb())
                {
                    Log.Info("Ensuring lists...");
                    Builder.EnsureLists(web);

                    Log.Info("Let the game begin!");

                    var game = new Game(web);

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

                    var commands = GetCommands(start);
                    commands = commands.Concat(GetCommands(typeof (ExtremeSharePoint)));

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

        private static IEnumerable<ConsoleCommand> GetCommands(Type start)
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(start);
        }
    }
}