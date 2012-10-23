using System.Text.RegularExpressions;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    public class Ip : IQuestion
    {
        #region IQuestion Members

        public bool Run(string line)
        {
            line = line.Trim();
            if (!Regex.IsMatch("0.0.0.0", line)) return false;
            if (!Regex.IsMatch("255.0.0.0", line)) return false;
            if (!Regex.IsMatch("0.255.0.0", line)) return false;
            if (!Regex.IsMatch("0.0.255.0", line)) return false;
            if (!Regex.IsMatch("0.0.0.255", line)) return false;
            if (!Regex.IsMatch("255.255.255.255", line)) return false;
            if (Regex.IsMatch("-1.0.0.0", line)) return false;
            if (Regex.IsMatch("256.0.0.0", line)) return false;
            if (Regex.IsMatch("0.256.0.0", line)) return false;
            if (Regex.IsMatch("0.0.256.0", line)) return false;
            if (Regex.IsMatch("0.0.0.256", line)) return false;
            if (Regex.IsMatch("256.256.256.256", line)) return false;
            return true;
        }

        public int Level
        {
            get { return 5; }
        }

        public string Question
        {
            get { return "Bygg og returner en RegExp som kan matche IP adresser"; }
        }

        #endregion
    }
}