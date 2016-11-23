using Sitecore.Modules.PushMenu.Models;
using Sitecore.Pipelines;
using System.Text;

namespace Sitecore.Modules.PushMenu.Pipelines.GetMenuHtml
{
    public class GetMenuHtmlArgs : PipelineArgs
    {
        public StringBuilder Html { get; set; }

        public MenuItem MenuItem { get; set; }
    }
}