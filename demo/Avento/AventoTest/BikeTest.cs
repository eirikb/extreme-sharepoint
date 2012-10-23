using System;
using System.Linq;
using System.Text.RegularExpressions;
using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AventoTest
{
    [TestClass]
    public class BikeTest
    {
        [TestMethod]
        public void QuestionTest()
        {
            Enumerable.Range(0, 1000).ToList().ForEach(i =>
                {
                    var q = new Bike();
                    Console.WriteLine(q.Question);
                    var nums = Regex.Matches(q.Question, @"\d+");
                    var length = int.Parse(nums[0].Value);
                    var speed = int.Parse(nums[1].Value);
                    var res = (length*10.0)/speed;
                    Console.WriteLine("Speed {0}, Length {1}, res {2}", speed, length, res);
                    Assert.IsTrue(q.Run("" + res));
                });
        }
    }
}