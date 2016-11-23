using Sitecore.Modules.PushMenu.Extensions;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using System.Text;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class MyWrapInContainer : WrapInContainer
    {
        public override StringBuilder Wrap(string htmlToWrap)
        {
            var result = new StringBuilder();
            result.Append("<nav class=\"ma5-menu-mobile\">");
            result.Append(
                "<div class=\"ma5-menu-header\"><a class=\"btn btn-primary ma5-toggle-menu\">Menu<span class=\"glyphicon glyphicon-menu-hamburger\"></span></a></div>");
            result.Append("<ul data-ma5-order=\"ma5-ul\">");
            result.Append(htmlToWrap);
            result.Append("</ul></nav>");
            return result;
        }
    }
}