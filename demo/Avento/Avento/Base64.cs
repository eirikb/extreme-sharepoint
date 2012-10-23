using System;
using System.Text;
using System.Text.RegularExpressions;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Base64 : IQuestion
    {
        public Base64()
        {
            const string q = "Nice job! Svar 'luretrix'";
            var bytes = Encoding.UTF8.GetBytes(q);
            Question = Convert.ToBase64String(bytes);
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            return Regex.IsMatch(line, @"^luretrix$", RegexOptions.IgnoreCase);
        }

        public int Level
        {
            get { return 8; }
        }

        public string Question { get; private set; }

        #endregion
    }
}