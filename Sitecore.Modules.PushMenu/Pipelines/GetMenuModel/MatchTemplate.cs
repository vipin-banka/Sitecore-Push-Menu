namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuModel
{
    public class MatchTemplate : GetMenuModelProcessorBase
    {
        public override void Process(GetMenuModelArgs args)
        {
            if (!args.Aborted)
            {
                if (string.IsNullOrEmpty(args.SiteSettings.AllowedTemplates)
                || !args.SiteSettings.AllowedTemplates.Contains(args.SitecoreItem.TemplateID.ToString()))
                {
                    args.AbortPipeline();
                }
            }
        }
    }
}