using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Modules.PushMenu.Models;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuModel;
using System.Linq;

namespace Sitecore.Modules.PushMenu.Managers
{
    public class MenuTreeManager
    {
        private readonly SiteSettings _siteSettings;

        private MenuItem _menuItem;

        public MenuTreeManager(SiteSettings siteSettings)
        {
            _siteSettings = siteSettings;
        }

        public MenuItem CreateTree()
        {
            var siteId = _siteSettings.SiteStartItemId;

            if (string.IsNullOrEmpty(siteId))
            {
                return null;
            }

            if (_siteSettings.Database == null)
            {
                return null;
            }

            var rootItem = _siteSettings.Database.GetItem(new ID(siteId));

            if (rootItem != null)
            {
                _menuItem = new MenuItem();
                AddSubMenu(rootItem, _menuItem);
                
                if (_menuItem.SubMenuItems != null && _menuItem.SubMenuItems.Count > 0)
                {
                    _menuItem = _menuItem.SubMenuItems.First();
                }
                else
                {
                    _menuItem = null;
                }
            }

            return _menuItem;
        }

        private void AddSubMenu(Item item, MenuItem menu)
        {
            if (item != null)
            {
                var args = new GetMenuModelArgs();
                args.SiteSettings = _siteSettings;
                args.SitecoreItem = item;
                GetMenuModelPipeline.Run(args);

                var menuItem = args.MenuItem;

                args.SitecoreItem = null;
                args.MenuItem = null;

                if (menuItem == null)
                {
                    return;
                }

                menu.SubMenuItems.Add(menuItem);

                if (item.Children != null
                    && item.Children.Count > 0)
                {
                    foreach (var child in item.Children.ToArray())
                    {
                        AddSubMenu(child, menuItem);
                    }
                }
            }
        }
    }
}