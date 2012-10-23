using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eirikb.SharePoint.Extreme.Lists;
using ManyConsole;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    public static class ExtremeSharePoint
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static Game Game;
        public static SPWeb Web;


        public static void Start(string url, Type start)
        {
            Log.Info("Connecting to SharePoint...");
            using (var site = new SPSite(url))
            {
                using (Web = site.OpenWeb())
                {
                    Log.Info("Ensuring lists...");
                    ListBuilder.EnsureLists(Web);

                    Log.Info("Let the game begin!");

                    Game = new Game(Web);

                    new Thread(() =>
                        {
                            while (true)
                            {
                                lock (Game)
                                {
                                    if (!Game.Run) break;
                                    Game.Ping();
                                    Monitor.Wait(Game, TimeSpan.FromSeconds(5));
                                    if (!Game.Run) break;
                                }
                            }
                        }).Start();

                    var commands = GetCommands(start);
                    commands = commands.Concat(GetCommands(typeof (ExtremeSharePoint)));

                    while (true)
                    {
                        var line = Console.ReadLine();
                        var cmd = new string[] {};
                        if (line != null) cmd = line.Split(' ');
                        var result = ConsoleCommandDispatcher.DispatchCommand(commands, cmd, Console.Out);
                        if (result < -1) break;
                    }
                    lock (Game)
                    {
                        Game.Run = false;
                        Monitor.Pulse(Game);
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