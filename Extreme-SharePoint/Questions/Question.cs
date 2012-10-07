using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eirikb.SharePoint.Extreme.Questions
{
    public interface IQuestion
    {
        int Level { get; }
        bool Run(string line);
    }

    public class Question
    {
        public static List<IQuestion> GetQuestions(int level)
        {
            return Assembly.GetExecutingAssembly().GetTypes().
                Where(t => t.GetInterfaces().Contains(typeof (IQuestion)) && t.GetConstructor(Type.EmptyTypes) != null).
                Select(Activator.CreateInstance).Cast<IQuestion>().
                Where(q => q.Level <= level).ToList();
        }

        public static IQuestion GetRandomQuestion(int level)
        {
            var questions = GetQuestions(level);
            return questions[new Random().Next(questions.Count)];
        }
    }
}