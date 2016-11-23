using Sitecore.Modules.PushMenu.Extensions;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using System.Text;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class WrapInContainer : GetMenuHtmlProcessorBase
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
            result.Append("<nav class=\"ma5-menu-mobile\"><ul class=\"ma5-ul\">");
            result.Append(htmlToWrap);
            result.Append("</ul></nav>");
            return result;
        }
    }
}