using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Modules.PushMenu.Extensions;
using Sitecore.Modules.PushMenu.Models;
using Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml;
using Sitecore.SecurityModel;
using Sitecore.Text;

namespace Sitecore.Modules.PushMenu.Managers
{
    public class PushMenuManager
    {
        private readonly Database _database;
        private readonly IStorageManager _storageManager;

        public PushMenuManager()
            : this(null)
        {
            _storageManager = Factory.CreateObject("pushmenu/storageManagerSettings", false) as IStorageManager;
        }

        public PushMenuManager(Database database)
        {
            _database = database;
            if (_database == null && Sitecore.Context.Database != null)
            {
                _database = Sitecore.Context.Database;
            }
        }

        public PushMenuSettings PushMenuSettings { get; set; }

        public void LoadSettings()
        {
            if (_database == null)
                return;

            var item = _database.GetItem(Constants.ItemIDs.PushMenuSettingItemId);

            if (item != null)
            {
                PushMenuSettings = item.MapToPushMenuSetting();
            }
        }

        public void Generate()
        {
            LoadSettings();
            if (PushMenuSettings != null)
            {
                var list = GetSiteSettingsList(PushMenuSettings);

                if (list == null)
                {
                    return;
                }

                using (var sd = new SecurityDisabler())
                {
                    foreach (var siteSettingItem in list)
                    {
                        Generate(siteSettingItem);
                    }
                }
            }
        }

        private void Generate(SiteSettings siteSettingItem)
        {
            if (!siteSettingItem.GenerateOnCall)
            {
                var tree = new MenuTreeManager(siteSettingItem).CreateTree();
                if (_storageManager != null)
                {
                    _storageManager.Write(siteSettingItem, tree);
                }
            }
        }

        private MenuItem GetContextSiteMenu()
        {
            LoadSettings();
            var siteSettings = GetContextSiteSettings(PushMenuSettings);

            if (siteSettings != null)
            {
                MenuItem tree;
                if (siteSettings.GenerateOnCall)
                {
                    tree = new MenuTreeManager(siteSettings).CreateTree();
                }
                else
                {
                    siteSettings.GenerateOnCall = true;
                    tree = _storageManager.Read(siteSettings) ?? new MenuTreeManager(siteSettings).CreateTree();
                }

                return tree;
            }

            return null;
        }

        public string GetContextSiteMenuHtml()
        {
            var menu = GetContextSiteMenu();

            if (menu != null && !string.IsNullOrEmpty(menu.Id))
            {
                var args = new GetMenuHtmlArgs
                {
                    Html = new StringBuilder(),
                    MenuItem = menu
                };

                GetMenuHtmlPipeline.Run(args);

                return args.Html != null ? args.Html.ToString() : string.Empty;
            }

            return string.Empty;
        }

        public IList<SiteSettings> GetSiteSettingsList(PushMenuSettings pushMenuSettings)
        {
            if (pushMenuSettings == null || string.IsNullOrEmpty(pushMenuSettings.SiteSettings))
            {
                return null;
            }

            var items = new ListString(pushMenuSettings.SiteSettings);

            var result = new List<SiteSettings>();
            foreach (var id in items)
            {
                var item = _database.GetItem(id);
                if (item != null && item.TemplateID == Constants.TemplateIDs.SiteSettingsId)
                {
                    result.Add(item.MapToSiteSettings());
                }
            }

            return result;
        }

        public SiteSettings GetContextSiteSettings(PushMenuSettings pushMenuSettings)
        {
            if (Context.Site == null)
            {
                return null;
            }

            var siteItem = Context.Site.SiteItem();

            if (siteItem != null)
            {
                var list = GetSiteSettingsList(pushMenuSettings);
                if (list != null)
                {
                    return
                        list.FirstOrDefault(
                            i => siteItem.ID.ToString().Equals(i.SiteStartItemId, StringComparison.OrdinalIgnoreCase));
                }
            }

            return null;
        }
    }
}