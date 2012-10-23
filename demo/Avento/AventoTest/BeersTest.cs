using System;
using System.Collections.Generic;
using System.Linq;
using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class BeersTest
    {
        private readonly Dictionary<string, bool> _beers = new Dictionary<string, bool>
            {
                {"Dahls", true},
                {"Frydelnund", true},
                {"Aass", true},
                {"Trio", true},
                {"Ægir", true},
                {"Fruens", false},
                {"Sulavann", false},
                {"Heimert", false},
                {"Tuborg", false},
                {"Sol", false}
            };

        [TestMethod]
        public void QuestionTest()
        {
            var q = new Beers();
            var beersStr = q.Question.Split(new[] {'?'})[1];
            var beers = beersStr.Trim().Split(new[] {' '});
            Console.WriteLine(string.Join(" ", beers));
            var all = beers.All(beer => _beers[beer]);
            Console.WriteLine(all);
            Assert.IsTrue(q.Run("" + all));
            Assert.IsFalse(q.Run("" + !all));
        }
    }
}