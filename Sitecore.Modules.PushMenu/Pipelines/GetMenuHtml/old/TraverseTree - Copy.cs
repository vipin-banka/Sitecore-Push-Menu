using System.Globalization;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Modules.PushMenu.Extensions;
using Sitecore.Modules.PushMenu.Models;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using System.Text;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class TraverseTree1 : GetMenuHtmlProcessorBase
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

        public virtual string ItemStartHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return string.Format("<li class=\"push-menu-level{0}\">", menuDetail.Level.ToString(CultureInfo.CurrentCulture));
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
            var iconHtml = string.Format("<i class=\"fa fa-tags {0}\"></i>", menuDetail.MenuItem.CssClass);

            if (!menuDetail.IsHome)
            {
                stringBuilder.Append(string.Format("<a href=\"{0}\">{1}{2}</a>", menuDetail.MenuItem.Url, iconHtml, menuDetail.MenuItem.GetText()));
            }

            if (menuDetail.HasChild)
            {
                stringBuilder.Append(string.Format("<h2 {2}>{0}{1}</h2>", iconHtml, menuDetail.MenuItem.GetText(), menuDetail.IsCurrentPage ? "class=\"init-menu\"" : ""));
            }

            return stringBuilder.ToString();
        }

        public virtual string LevelStartHtml(MenuDetail menuDetail)
        {
            if (!menuDetail.IsHome)
            {
                return "<ul>";
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