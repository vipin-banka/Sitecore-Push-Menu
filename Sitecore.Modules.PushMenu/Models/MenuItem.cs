using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Modules.PushMenu.Models
{
    public class MenuItem
    {
        private string _altText = string.Empty;

        private IList<MenuItem> _subMenuItems = new List<MenuItem>();
 
        public string Id { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public string CssClass { get; set; }

        public IList<MenuItem> SubMenuItems
        {
            get { return _subMenuItems; }
        }

        public IDictionary<string, string> LanguageTexts = new Dictionary<string, string>();

        public string AltText
        {
            get
            {
                if (!string.IsNullOrEmpty(_altText))
                {
                    return _altText;
                }

                return GetText();
            }

            set { _altText = value; }
        }

        public MenuItem AssignSubMenu(IList<MenuItem> items)
        {
            _subMenuItems = items;
            return this;
        }

        public string GetText()
        {
            var l = Sitecore.Context.Language;
            if (l != null)
            {
                var lname = l.Name;
                if (LanguageTexts != null && LanguageTexts.Any() && LanguageTexts.ContainsKey(lname))
                {
                    return LanguageTexts[lname];
                }
            }

            return Text;
        }
    }
}