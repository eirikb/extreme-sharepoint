using System;
using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class FindWaldoTest
    {
        [TestMethod]
        public void QuestionTest()
        {
            var q = new FindWaldo();
            Console.WriteLine(q.Question);
        }
    }
}