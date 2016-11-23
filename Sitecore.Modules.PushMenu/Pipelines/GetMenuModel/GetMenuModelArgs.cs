using Sitecore.Data.Items;
using Sitecore.Modules.PushMenu.Models;
using Sitecore.Pipelines;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuModel
{
    public class GetMenuModelArgs : PipelineArgs
    {
        public SiteSettings SiteSettings { get; set; }

        public Item SitecoreItem { get; set; }

        public MenuItem MenuItem { get; set; }
    }
}
