using System;
using System.Text.RegularExpressions;
using Avento;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AventoTest
{
    [TestClass]
    public class FibonacciTest
    {
        [TestMethod]
        public void SimpleFibonacciTest()
        {
            var res = new[]
                {
                    1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711,
                    28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040, 1346269, 2178309, 3524578, 5702887,
                    9227465, 14930352, 24157817, 39088169
                };
            var i = 0;
            res.ToList().ForEach(fib => Assert.AreEqual(Fibonacci.GetFibonacci(i++), fib));
        }
        
        [TestMethod]
        public void QuestionTest()
        {
            var q = new Fibonacci();
            var n = int.Parse(Regex.Match(q.Question, @"\d+").Value);
            var res = GetFibbonacci(n);
            Assert.IsTrue(q.Run("" + res));
            Assert.IsFalse(q.Run("" + (res - 1)));
        }

        private static int GetFibbonacci(int n)
        {
            var num1 = 0;
            var num2 = 1;
            for (var i = 0; i < 100000; i++)
            {
                var sum = num1 + num2;
                num1 = num2;
                num2 = sum;
                if (num1 == n) return i;
                if (num1 > n) return -1;
            }
            return -1;
        }
    }
}