using System;
using System.Collections.Generic;
using System.Linq;
using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class TopperTest
    {
        private readonly Dictionary<string, int> _topps = new Dictionary<string, int>
            {
                {"Sukkertind", 876},
                {"Sulafjellet", 725},
                {"Godøyfjellet", 497},
                {"Gamlemsveten", 790},
                {"Slogen", 1564},
                {"Lauparen", 1434},
                {"Prekestolen", 604},
                {"Galdhøpiggen", 2469}
            };

        [TestMethod]
        public void QuestionTest()
        {
            Enumerable.Range(0, 100).ToList().ForEach(i =>
                {
                    var q = new Topper();
                    var s = q.Question;
                    Console.WriteLine(q.Question);
                    s = s.Substring(s.IndexOf(":", StringComparison.Ordinal) + 1).Trim();

                    var tops = s.Split(' ').ToList();
                    tops.Sort((a, b) => _topps[a] - _topps[b]);
                    s = string.Join(" ", tops.ToArray());
                    Console.WriteLine(s);
                    Assert.IsTrue(q.Run(s));
                });
        }
    }
}