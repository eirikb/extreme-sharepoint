using System;
using System.Net;
using Microsoft.SharePoint;
using log4net;

namespace Eirikb.SharePoint.Extreme
{
    internal static class Client
    {
        private static readonly ILog Log = LogManager.GetLogger("Extreme-SharePoint");

        public static string Request(SPListItem team, IQuestion question)
        {
            var host = "" + team["Host"];
            string result = null;
            using (var client = new WebClient())
            {
                var url = new UriBuilder(host) {Query = question.Question}.Uri.AbsoluteUri;
                if (!url.EndsWith("/")) url += "/";
                try
                {
                    Log.DebugFormat("Sending request to {0}", url);
                    result = client.DownloadString(url);
                }
                catch (Exception e)
                {
                    Log.Error("Request to team " + team.Title, e);
                }
                Log.DebugFormat("Got result: {0}", result);
            }
            return result;
        }
    }
}