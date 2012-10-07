using ManyConsole;

namespace Eirikb.SharePoint.Extreme.commands
{
    public class Exit : ConsoleCommand
    {
        public Exit()
        {
            IsCommand("exit", "Exit the application");
        }

        public override int Run(string[] remainingArguments)
        {
            return -2;
        }
    }
}