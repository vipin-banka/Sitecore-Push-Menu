using Sitecore.Diagnostics;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using Sitecore.Pipelines;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class GetMenuHtmlPipeline
    {
        public static void Run(GetMenuHtmlArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            CorePipeline.Run("pushmenu.getmenuhtml", (PipelineArgs)args);
        }
    }
}
