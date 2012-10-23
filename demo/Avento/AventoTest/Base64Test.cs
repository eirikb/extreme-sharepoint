using System;
using System.Text;
using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class Base64Test
    {
        [TestMethod]
        public void QuestionTest()
        {
            var q = new Base64();
            var bytes = Convert.FromBase64String(q.Question);
            var a = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(a);
            Assert.IsTrue(a.Contains("'luretrix'"));
            Assert.IsTrue(q.Run("luretrix"));
            Assert.IsFalse(q.Run("sluretrix"));
        }
    }
}