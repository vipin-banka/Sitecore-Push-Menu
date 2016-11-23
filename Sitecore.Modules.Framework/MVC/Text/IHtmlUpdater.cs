using System.Text;

namespace Sitecore.Modules.Framework.MVC.Text
{
    public interface IHtmlUpdater
    {
        bool UpdateHtml(StringBuilder html);
    }
}