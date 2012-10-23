using System;
using Avento;
using Ciloci.Flee;
using Eirikb.SharePoint.Extreme;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class CalculusTest
    {
        [TestMethod]
        public void Level1Test()
        {
            var c1 = new Calculus1();
            Test(c1);
        }

        [TestMethod]
        public void Level2Test()
        {
            var c2 = new Calculus2();
            Test(c2);
        }

        [TestMethod]
        public void Level3Test()
        {
            var c3 = new Calculus3();
            Test(c3);
        }

        private static void Test(IQuestion qs)
        {
            Console.WriteLine(qs.Question);

            var q = qs.Question;
            q = q.Replace("pluss", "+").
                Replace("minus", "-").
                Replace("ganger", "*").
                Replace("delt på", "/").
                Replace("Hva er", "");
            Console.WriteLine(q);
            var context = new ExpressionContext();
            context.Imports.AddType(typeof (Math));
            var eDynamic = context.CompileDynamic(q);
            var res = eDynamic.Evaluate();
            Assert.IsTrue(qs.Run("" + res));
        }
    }
}