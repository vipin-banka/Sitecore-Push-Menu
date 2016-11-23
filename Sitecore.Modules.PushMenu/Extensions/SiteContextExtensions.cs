using Sitecore.Data.Items;
using Sitecore.Sites;

namespace Sitecore.Modules.PushMenu.Extensions
{
    public static class SiteContextExtensions
    {
        public static Item SiteItem(this SiteContext context)
        {
            var rootPath = Context.Site.StartPath;

            if (string.IsNullOrEmpty(rootPath))
            {
                return null;
            }

            return Context.Database.GetItem(rootPath);
        }
    }
}
