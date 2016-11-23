using System.Collections.Generic;
using Newtonsoft.Json;
using Sitecore.Modules.PushMenu.Models;

namespace Sitecore.Modules.PushMenu.Managers
{
    public class InMemoryStorageManager : IStorageManager
    {
        private static IDictionary<string, IDictionary<string, string>> list = new Dictionary<string, IDictionary<string, string>>();
        private static readonly object LockObject = new object();
        private SiteSettings _settings;

        private void Initialize(SiteSettings settings)
        {
            lock (LockObject)
            {
                _settings = settings;
                if (!list.ContainsKey(_settings.SiteStartItemId))
                {
                    list.Add(_settings.SiteStartItemId, new Dictionary<string, string>());
                }

                if (!list[_settings.SiteStartItemId].ContainsKey(_settings.Database.Name))
                {
                    list[_settings.SiteStartItemId].Add(_settings.Database.Name, string.Empty);
                }
            }
        }

        public void Write(SiteSettings settings, MenuItem tree)
        {
            Initialize(settings);
            if (tree != null)
            {
                list[_settings.SiteStartItemId][_settings.Database.Name] = JsonConvert.SerializeObject(tree);
            }
            else
            {
                list[_settings.SiteStartItemId][_settings.Database.Name] = string.Empty;
            }
        }

        public MenuItem Read(SiteSettings settings)
        {
            Initialize(settings);
            var json = list[_settings.SiteStartItemId][_settings.Database.Name];

            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<MenuItem>(json);
            }

            return null;
        }
    }
}