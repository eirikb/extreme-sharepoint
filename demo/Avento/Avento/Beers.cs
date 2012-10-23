using System;
using System.Collections.Generic;
using System.Linq;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Beers : IQuestion
    {
        private readonly bool _all;

        private readonly Dictionary<string, bool> _beers = new Dictionary<string, bool>
            {
                {"Dahls", true},
                {"Frydelnund", true},
                {"Aass", true},
                {"Trio", true},
                {"Ægir", true},
                {"Sol", false},
                {"Fruens", false},
                {"Sulavann", false},
                {"Tuborg", false},
                {"Heimert", false}
            };

        public Beers()
        {
            var count = new Random().Next(1, _beers.Keys.Count - 2);
            var keys = _beers.Keys.ToList();
            keys = Sorder.RandomPermutation(keys).ToList();
            keys = keys.Take(count).ToList();
            Question = string.Format("Er alle disse norske bryggerier(ish)? {0}", string.Join(" ", keys.ToArray()));
            _all = keys.All(key => _beers[key]);
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            line = line.Trim();
            bool t;
            if (!bool.TryParse(line, out t)) return false;
            return t == _all;
        }

        public int Level
        {
            get { return 6; }
        }

        public string Question { get; private set; }

        #endregion
    }
}