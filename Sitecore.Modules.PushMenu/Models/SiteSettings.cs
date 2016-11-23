using Sitecore.Data;

namespace Sitecore.Modules.PushMenu.Models
{
    public class SiteSettings
    {
        public string SiteStartItemId { get; set; }

        public string AllowedTemplates { get; set; }

        public bool GenerateOnCall { get; set; }

        public Database Database { get; set; }
    }
}