using System;
using System.Collections.Generic;
using Eirikb.SharePoint.Extreme;
using System.Linq;

namespace Avento
{
    public class FindWaldo : IQuestion
    {
        private readonly Dictionary<int, string[]> _waldos = new Dictionary<int, string[]>
            {
                {-1, new[] {"Hans-Petter", "Erna", "Marius"}},
                {3, new[] {"Ottar", "Eirik Peirik", "Knut", "Christian E", "Terje Johan", "Hans Petter", "Stig", "Erik", "Håkon", "Line", "Gry"}},
                {4, new[] {"Finn", "Øystein", "Kenneth", "Christian H", "Terje"}}
            };

        private readonly int _floor;

        public FindWaldo()
        {
            var floors = _waldos.Keys.ToList();
            _floor = floors[new Random().Next(floors.Count)];
            var floorWaldos = _waldos[_floor];
            var waldo = floorWaldos[new Random().Next(floorWaldos.Length)];
            Question = string.Format("I hvilken etasje sitter {0}? -1 for ingen etasje", waldo);
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            return line.Trim() == "" + _floor;
        }

        public int Level { get { return 3; } }
        public string Question { get; private set; }

        #endregion
    }
}