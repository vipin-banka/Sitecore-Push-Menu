using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Events;
using Sitecore.Modules.PushMenu.Managers;
using System;
using Sitecore.Tasks;

namespace Sitecore.Modules.PushMenu
{
    public class PushMenuHandler
    {
        public void RefreshPushMenuJson(object sender, EventArgs args)
        {
            var sitecoreEventArgs = args as SitecoreEventArgs;
            var database = GetDatabse(sitecoreEventArgs);
            if (database == null)
            {
                return;
            }

            PushMenuActionManager.AddAction(database.Name);
        }

        private Database GetDatabse(SitecoreEventArgs args)
        {
            var indexName = ((Sitecore.Events.SitecoreEventArgs)(args)).Parameters[0] as string;

            var crawlers =
                ContentSearchManager.GetIndex(indexName).Crawlers as
                    System.Collections.Generic.List<Sitecore.ContentSearch.IProviderCrawler>;

            if (crawlers != null && crawlers.Count > 0)
            {
                var crawler = crawlers[0] as Sitecore.ContentSearch.SitecoreItemCrawler;
                if (crawler != null && !string.IsNullOrEmpty(crawler.Database))
                {
                    return Factory.GetDatabase(crawler.Database);
                }
            }

            return null;
        }
    }
}