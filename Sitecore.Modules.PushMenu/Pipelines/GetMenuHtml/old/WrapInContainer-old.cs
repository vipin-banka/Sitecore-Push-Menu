using Sitecore.Modules.PushMenu.Extensions;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using System.Text;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public abstract class WrapInContainer : GetMenuHtmlProcessorBase
    {
        public override void Process(GetMenuHtmlArgs args)
        {
            if (!args.Aborted)
            {
                args.Html = Wrap(args.Html.ToString());
            }
        }

        public virtual StringBuilder Wrap(string htmlToWrap)
        {
            var result = new StringBuilder();
            result.Append("<div id=\"menu\"><nav>");
            result.Append(htmlToWrap);
            result.Append("</nav></div>");
            return result;
        }
    }
}