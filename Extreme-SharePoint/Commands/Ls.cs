using System;
using ManyConsole;

namespace Eirikb.SharePoint.Extreme.Commands
{
    public class Ls : ConsoleCommand
    {
        public Ls()
        {
            IsCommand("ls", "List all users");
        }

        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine("Hello world");
            return 0;
        }
    }
}