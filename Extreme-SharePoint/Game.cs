using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Eirikb.SharePoint.Extreme
{

    class Game
    {
        public bool Run { get; set; }
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public void Ping()
        {
            Log.Info("Game pinged");
            Console.WriteLine("OMG!");
        }
    }
}
