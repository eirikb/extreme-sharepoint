using System;
using System.Linq;
using ManyConsole;
using Microsoft.SharePoint;
using NecroNet.SharePoint.CodeCaml;
using log4net;

namespace Eirikb.SharePoint.Extreme.Commands
{
    public class LastFail : ConsoleCommand
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public LastFail()
        {
            IsCommand("lsf", "Last failing questions");
        }

        public override int Run(string[] remainingArguments)
        {
            var web = ExtremeSharePoint.Web;
            if (web == null)
            {
                Log.Warn("SPWeb in ExtremeSharpoint is not set");
                return 0;
            }
            var query = new SPQuery {Query = CQ.Where(CQ.Eq.FieldRef("Success").Value(false)), RowLimit = 10};

            var questions = web.Lists["Stats"].GetItems(query);
            questions.Cast<SPListItem>().ToList().ForEach(
                q => Console.WriteLine("{0} - {1} - {2} - {3}", q["Team"], q["Question"], q["Answer"], q["Points"]));
            return 0;
        }
    }
}