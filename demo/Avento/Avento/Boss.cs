using System.Text.RegularExpressions;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Boss : IQuestion
    {
        #region IQuestion Members

        public bool Run(string line)
        {
            return Regex.IsMatch(line, @"^stian\ bang$", RegexOptions.IgnoreCase);
        }

        public int Level
        {
            get { return 1; }
        }

        public string Question
        {
            get { return "Hvem er sjefen i avento?"; }
        }

        #endregion
    }
}
