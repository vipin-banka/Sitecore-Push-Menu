using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Modules.PushMenu.Models;

namespace Sitecore.Modules.PushMenu.Extensions
{
    public static class ItemExtensions
    {
        public static MenuItem MapToMenuItem(this Item item, bool getAllLanguageData)
        {
            var result = new MenuItem
            {
                Id = item.ID.ToString(),
                Text = item.DisplayName,
                Url = LinkManager.GetItemUrl(item)
            };

            if (getAllLanguageData)
            {
                foreach (var itemLanguage in item.Languages)
                {
                    var litem = item.Database.GetItem(item.ID, itemLanguage);
                    if (litem != null)
                    {
                        result.LanguageTexts.Add(itemLanguage.Name, litem.DisplayName);
                    }
                }
            }

            return result;
        }

        public static PushMenuSettings MapToPushMenuSetting(this Item item)
        {
            return new PushMenuSettings
            {
                SiteSettings = item.Fields[Constants.FieldIDs.PushMenuSettings.Sites].Value
            };
        }

        public static SiteSettings MapToSiteSettings(this Item item)
        {
            return new SiteSettings
            {
                SiteStartItemId = item.Fields[Constants.FieldIDs.SiteSettings.SiteStartPath].Value,
                AllowedTemplates = item.Fields[Constants.FieldIDs.SiteSettings.AllowedTemplates].Value,
                GenerateOnCall = item.Fields[Constants.FieldIDs.SiteSettings.GenerateOnCall].Value == "1",
                Database = item.Database
            };
        }
    }
}
