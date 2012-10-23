using System;
using System.Collections.Generic;
using System.Linq;

namespace Avento
{
    public static class Sorder
    {
        private static readonly Random random = new Random();

        public static IEnumerable<T> RandomPermutation<T>(IEnumerable<T> sequence)
        {
            var retArray = sequence.ToArray();


            for (var i = 0; i < retArray.Length - 1; i += 1)
            {
                var swapIndex = random.Next(i + 1, retArray.Length);
                var temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }

            return retArray;
        }
    }
}