using System;
using System.Collections.Generic;
using System.Linq;

namespace Eirikb.SharePoint.Extreme
{
    public interface IQuestion
    {
        int Level { get; }
        string Question { get; }
        int Run(string line);
    }

    public static class Question
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
            return questions.Count == 0 ? null : questions[new Random().Next(questions.Count)];
        }
    }
}