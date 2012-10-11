using System;
using ManyConsole;

namespace Eirikb.SharePoint.Extreme.Commands
{
    internal class Questions : ConsoleCommand
    {
        public Questions()
        {
            IsCommand("lsq", "List all questoins");
        }

        public override int Run(string[] remainingArguments)
        {
            var questions = Question.GetQuestions(ExtremeSharePoint.Game.Level);
            questions.ForEach(q => Console.WriteLine("{0} - {1} - {2}", q.Level, q.GetType(), q.Question));
            return 0;
        }
    }
}