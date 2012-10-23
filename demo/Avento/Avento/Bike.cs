using System;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Bike : IQuestion
    {
        private readonly double _res;

        public Bike()
        {
            var speed = new Random().Next(10, 90);
            var length = new Random().Next(3, 20);
            _res = (length * 10.0) / speed;
            Question =
                string.Format(
                    "Hvor lang tid bruker Stian og Ottar på å sykle {0} mil med en snitthastighet på {1} km/t?",
                    length, speed);
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            double d;
            if (!double.TryParse(line, out d)) return false;
            return d - 0.5 < _res && d + 0.5 > _res;
        }

        public int Level
        {
            get { return 2; }
        }

        public string Question { get; private set; }

        #endregion
    }
}