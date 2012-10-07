using System;
using ManyConsole;

namespace Eirikb.SharePoint.Extreme.Commands
{
    public class Exit : ConsoleCommand
    {
        public Exit()
        {
            IsCommand("exit", "Exit the application");
        }

        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine("Good bye proud knight");
            return -2;
        }
    }
}