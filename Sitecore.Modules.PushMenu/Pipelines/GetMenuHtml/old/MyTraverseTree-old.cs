using System.Globalization;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Modules.PushMenu.Extensions;
using Sitecore.Modules.PushMenu.Models;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using System.Text;
using Sitecore.Pipelines.WebDAV.Processors;
using Sitecore.Web.UI.HtmlControls;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class MyTraverseTree : TraverseTree
    {
        private static string GetClassSuffix(MenuDetail menuDetail)
        {
            var level = string.Empty;
            if (menuDetail != null && menuDetail.Parent != null && !menuDetail.IsHome)
            {
                var md = menuDetail;
                do
                {
                    level = "-" + md.Index.ToString(CultureInfo.InvariantCulture) + level;
                    md = md.Parent;
                } while (md != null && md.Parent != null && !md.IsHome);
            }

            return level;
        }

        public override string ItemStartHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                var result = new StringBuilder();
                result.Append(string.Format("<li data-ma5-order=\"ma5-li{0}\">", GetClassSuffix(menuDetail)));
                if (menuDetail.Index == 1 && !menuDetail.IsHome && !menuDetail.IsImmediateChildofHome)
                {
                    result.Append("<div class=\"ma5-leave-bar\"><span class=\"ma5-btn-leave\"><span class=\"glyphicon glyphicon-menu-left\" aria-hidden=\"true\"></span></span>Back</div>");
                }
                return result.ToString();
            }

            return string.Empty;
        }

        public override string ItemEndHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return "</li>";
            }

            return string.Empty;
        }

        public override string GetItemHtml(MenuDetail menuDetail)
        {
            var stringBuilder = new StringBuilder();

            if (!menuDetail.IsHome)
            {
                stringBuilder.Append(string.Format("<a class=\"ma5-path-to-active{0}\" href=\"{1}\">{2}</a>",
                    menuDetail.HasChild ? " ma5-has-submenu" : string.Empty,
                    menuDetail.MenuItem.Url,
                    menuDetail.MenuItem.GetText()));

                if (menuDetail.HasChild)
                {
                    stringBuilder.Append(
                        "<span class=\"ma5-btn-enter\"><span class=\"glyphicon glyphicon-menu-right\" aria-hidden=\"true\"></span></span>");
                }
            }

            return stringBuilder.ToString();
        }

        public override string LevelStartHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return string.Format("<ul class=\"{0}\" data-ma5-order=\"ma5-ul{1}\">", menuDetail.IsCurrentPage ? "ma5-active-ul" : "", GetClassSuffix(menuDetail));
            }

            return string.Empty;
        }

        public override string LevelEndHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return "</ul>";
            }

            return string.Empty;
        }
    }
}