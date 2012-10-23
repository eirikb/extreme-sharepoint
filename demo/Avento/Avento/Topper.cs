using System;
using System.Collections.Generic;
using System.Linq;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Topper : IQuestion
    {
        private readonly Dictionary<string, int> _topps = new Dictionary<string, int>
            {
                {"Sukkertind", 876},
                {"Sulafjellet", 725},
                {"Godøyfjellet", 497},
                {"Gamlemsveten", 790},
                {"Slogen", 1564},
                {"Lauparen", 1434},
                {"Prekestolen", 604},
                {"Galdhøpiggen", 2469}
            };

        private readonly List<string> _tops;

        public Topper()
        {
            var maxTops = new Random().Next(1, _topps.Keys.Count);
            _tops = _topps.Keys.ToList();
            _tops = Sorder.RandomPermutation(_tops).ToList();
            _tops = _tops.Take(maxTops).ToList();
            Question = string.Format("Sorter disse fjelltoppene i stigende rekkefølge: {0}",
                                     string.Join(" ", _tops.ToArray()));
            _tops.Sort((a, b) => _topps[a] - _topps[b]);
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            var c = 0;
            return line.Trim().Split(' ').All(s => _tops[c++] == s);
        }

        public int Level { get { return 4; } }
        public string Question { get; private set; }

        #endregion
    }
}