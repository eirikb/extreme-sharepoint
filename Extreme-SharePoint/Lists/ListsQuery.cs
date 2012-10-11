using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace Eirikb.SharePoint.Extreme.Lists
{
    public static class ListsQuery
    {
        public static List<SPListItem> GetTeams(SPWeb web)
        {
            return web.Lists["Teams"].GetItems(new SPQuery()).Cast<SPListItem>().ToList();
        }
    }
}