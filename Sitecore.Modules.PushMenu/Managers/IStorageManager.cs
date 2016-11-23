using Sitecore.Modules.PushMenu.Models;

namespace Sitecore.Modules.PushMenu.Managers
{
    public interface IStorageManager
    {
        void Write(SiteSettings settings, MenuItem tree);
        MenuItem Read(SiteSettings settings);
    }
}