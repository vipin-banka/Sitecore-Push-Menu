using System.IO;
using Newtonsoft.Json;
using Sitecore.Modules.PushMenu.Models;

namespace Sitecore.Modules.PushMenu.Managers
{
    public class FileStorageManager : IStorageManager
    {
        private string _dataFolder;
        private string _jsonFolderPath;
        private string _jsonFilePath;
        private SiteSettings _settings;
        private static readonly object LockObject = new object();

        private void SetPath(SiteSettings settings)
        {
            lock (LockObject)
            {
                _settings = settings;
                _dataFolder = Sitecore.Configuration.Settings.DataFolder;
                string folder = Path.Combine(_dataFolder, "push-menu-module", _settings.SiteStartItemId,
                    _settings.Database.Name);
                string file = Path.Combine(folder, "menu.json");
                _jsonFolderPath = folder;
                _jsonFilePath = file;
            }
        }

        public void Write(SiteSettings settings, MenuItem tree)
        {
            SetPath(settings);

            if (!System.IO.Directory.Exists(_dataFolder))
            {
                return;
            }

            if (!System.IO.Directory.Exists(_jsonFolderPath))
            {
                System.IO.Directory.CreateDirectory(_jsonFolderPath);
            }

            if (File.Exists(_jsonFilePath))
            {
                File.Delete(_jsonFilePath);
            }

            string json = JsonConvert.SerializeObject(tree);

            if (!string.IsNullOrEmpty(json))
            {
                File.WriteAllText(_jsonFilePath, json);
            }
        }

        public MenuItem Read(SiteSettings settings)
        {
            SetPath(settings);

            if (!System.IO.Directory.Exists(_dataFolder))
            {
                return null;
            }

            if (!System.IO.Directory.Exists(_jsonFolderPath))
            {
                return null;
            }

            if (!File.Exists(_jsonFilePath))
            {
                return null;
            }

            var json = File.ReadAllText(_jsonFilePath);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<MenuItem>(json);
        }
    }
}