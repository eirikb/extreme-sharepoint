using System;
using System.Collections.Generic;
using System.Linq;

namespace Eirikb.SharePoint.Extreme
{
    public interface IQuestion
    {
        int Level { get; }
        int Run(string line);
        string Question { get; }
    }

    public class Question
    {
        public static List<IQuestion> GetQuestions(int level)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies().ToList()
                    .SelectMany(s => s.GetTypes())
                    .Where(
                        t => t.GetInterfaces().Contains(typeof (IQuestion)) && t.GetConstructor(Type.EmptyTypes) != null)
                    .Select(Activator.CreateInstance).Cast<IQuestion>()
                    .Where(q => q.Level <= level).ToList();
        }

        public static IQuestion GetRandomQuestion(int level)
        {
            var questions = GetQuestions(level);
            if (questions.Count == 0) return null;
            return questions[new Random().Next(questions.Count)];
        }
    }
}