using System;
using System.Net;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    internal static class Client
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static void Request(SPListItem team, IQuestion question, DownloadStringCompletedEventHandler callback)
        {
            var host = "" + team["Host"];
            using (var client = new WebClient())
            {
                var query = string.Format("q={0}", question.Question);
                var url = new UriBuilder(host) {Query = query}.Uri;
                try
                {
                    Log.DebugFormat("Sending request to {0}", url);
                    client.DownloadStringAsync(url);
                    client.DownloadStringCompleted += callback;
                }
                catch (Exception e)
                {
                    Log.Error("Request to team " + team.Title, e);
                }
            }
        }
    }
}