using Sitecore.Modules.PushMenu.Models;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class TraverseTree : GetMenuHtmlProcessorBase
    {
        public override void Process(GetMenuHtmlArgs args)
        {
            if (!args.Aborted)
            {
                Traverse(args.MenuItem, args.Html, 0, 1, null);
            }
        }

        private void Traverse(MenuItem menu, StringBuilder menuHtml, int level, int index, MenuDetail detail)
        {
            var menuDetail = new MenuDetail
            {
                MenuItem = menu,
                Level = level,
                HasChild = menu.SubMenuItems != null
                               && menu.SubMenuItems.Count > 0,
                IsHome = IsHome(level),
                IsImmediateChildofHome = IsHomeImmediateChild(level),
                Index = index,
                Parent = detail
            };

            menuDetail.IsCurrentPage = IsCurrentPage(menuDetail);

            menuHtml.Append(ItemStartHtml(menuDetail));

            menuHtml.Append(GetItemHtml(menuDetail));

            if (menuDetail.HasChild)
            {
                menuHtml.Append(LevelStartHtml(menuDetail));
                var itemNumber = 0;
                foreach (var subMenuItem in menuDetail.MenuItem.SubMenuItems)
                {
                    itemNumber++;
                    Traverse(subMenuItem, menuHtml, level + 1, itemNumber, menuDetail);
                }

                menuHtml.Append(LevelEndHtml(menuDetail));
            }

            menuHtml.Append(ItemEndHtml(menuDetail));
        }

        private bool IsCurrentPage(MenuDetail menuDetail)
        {
            var id = Context.Item.ID.ToString();

            if (menuDetail.MenuItem.Id.Equals(id) && menuDetail.HasChild)
            {
                return true;
            }

            if (menuDetail.MenuItem.SubMenuItems != null)
            {
                var submenu = menuDetail.MenuItem.SubMenuItems.FirstOrDefault(i => i.Id.Equals(id));
                return (submenu != null && (submenu.SubMenuItems == null || submenu.SubMenuItems.Count <= 0));
            }

            return false;
        }

        private static bool IsHome(int level)
        {
            return level == 0;
        }

        private static bool IsHomeImmediateChild(int level)
        {
            return level == 1;
        }

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

        public virtual string ItemStartHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                var result = new StringBuilder();
                result.Append(string.Format("<li class=\"ma5-li{0}\">", GetClassSuffix(menuDetail)));
                if (menuDetail.Index == 1 && !menuDetail.IsHome && !menuDetail.IsImmediateChildofHome)
                {
                    result.Append(
                        "<div class=\"ma5-leave-bar\"><span class=\"ma5-btn-leave\"><i aria-hidden=\"true\" class=\"fa fa-arrow-left menu-arrow\"></i></span>");
                    result.Append(menuDetail.Parent.Parent.MenuItem.GetText());
                    result.Append("</div>");

                    result.Append("<div class=\"ma5-current-item-bar\">");
                    //result.Append(string.Format("<a href=\"{0}\">{1}</a>", menuDetail.MenuItem.Url, menuDetail.MenuItem.Text));
                    result.Append(string.Format("<a href=\"{0}\">{1}</a>", menuDetail.Parent.MenuItem.Url, menuDetail.Parent.MenuItem.GetText()));
                    result.Append("</div>");
                }

                return result.ToString();
            }

            return string.Empty;
        }

        public virtual string ItemEndHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return "</li>";
            }

            return string.Empty;
        }

        public virtual string GetItemHtml(MenuDetail menuDetail)
        {
            var stringBuilder = new StringBuilder();

            if (!menuDetail.IsHome)
            {
                stringBuilder.Append(string.Format("<a class=\"ma5-path-to-active\" href=\"{0}\">{1}</a>",
                    menuDetail.MenuItem.Url,
                    menuDetail.MenuItem.GetText()));

                if (menuDetail.HasChild)
                {
                    stringBuilder.Append(
                        "<span class=\"ma5-btn-enter\"><i aria-hidden=\"true\" class=\"fa fa-arrow-right menu-arrow\"></i></span>");
                }
            }

            return stringBuilder.ToString();
        }

        public virtual string LevelStartHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return string.Format("<ul class=\"{0}ma5-ul{1}\">", menuDetail.IsCurrentPage ? "ma5-active-ul " : "", GetClassSuffix(menuDetail));
            }

            return string.Empty;
        }

        public virtual string LevelEndHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return "</ul>";
            }

            return string.Empty;
        }
    }
}