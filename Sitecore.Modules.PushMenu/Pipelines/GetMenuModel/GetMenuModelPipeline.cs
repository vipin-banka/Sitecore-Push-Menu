using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuModel
{
    public class GetMenuModelPipeline
    {
        public static void Run(GetMenuModelArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            CorePipeline.Run("pushmenu.getmenumodel", (PipelineArgs)args);
        }
    }
}
