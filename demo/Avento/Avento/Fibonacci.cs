using System;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Fibonacci : IQuestion
    {
        private readonly int _random;

        public Fibonacci()
        {
            _random = new Random().Next(10);
            var fibonacci = GetFibonacci(_random);
            Question = string.Format("På hvilken plass i Fibonaccirekka ligger tallet {0}? (fra 1)", fibonacci);
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            return line.Trim() == "" + _random;
        }

        public int Level
        {
            get { return 7; }
        }

        public string Question { get; private set; }

        #endregion

        public static int GetFibonacci(int n)
        {
            var num1 = 0;
            var num2 = 1;
            for (var i = 0; i < n; i++)
            {
                var sum = num1 + num2;
                num1 = num2;
                num2 = sum;
            }
            return num2;
        }
    }
}