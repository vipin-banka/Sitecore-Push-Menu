using Sitecore.Modules.PushMenu.Managers;
using Sitecore.Modules.PushMenu.Models;
using System;
using System.Linq;
using System.Text;

namespace Sitecore.PushMenu.Web.layouts.SitecorePushMenu.Sublayouts
{
    public partial class PushMenu : System.Web.UI.UserControl
    {
        public string Html { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Html = new PushMenuManager().GetContextSiteMenuHtml();
        }
    }
}