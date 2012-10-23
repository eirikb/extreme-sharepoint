using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class IpTest
    {
        [TestMethod]
        public void QuestionTest()
        {
            var q = new Ip();
            Assert.IsTrue(
                q.Run(
                    @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"));
            Assert.IsFalse(q.Run(@"\d+\.\d+\.\d+\.\d+\."));
        }
    }
}