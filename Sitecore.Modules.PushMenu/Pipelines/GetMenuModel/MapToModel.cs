using Sitecore.Modules.PushMenu.Extensions;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuModel
{
    public class MapToModel : GetMenuModelProcessorBase
    {
        public override void Process(GetMenuModelArgs args)
        {
            if (!args.Aborted && args.MenuItem == null)
            {
                args.MenuItem = args.SitecoreItem.MapToMenuItem(!args.SiteSettings.GenerateOnCall);
            }
        }
    }
}