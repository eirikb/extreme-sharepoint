using Eirikb.SharePoint.Extreme;

namespace Avento
{
    internal class Program
    {
        private static void Main()
        {
            ExtremeSharePoint.Start("http://localhost", typeof(Program));
        }
    }
}