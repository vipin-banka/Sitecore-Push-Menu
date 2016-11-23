using System.Text;

namespace Sitecore.Modules.Framework.MVC.Text
{
    public abstract class HtmlUpdaterBase : IHtmlUpdater
    {
        public abstract bool UpdateHtml(StringBuilder html);

        protected int IndexOfHeadEnd(StringBuilder html)
        {
            return this.IndexOf(html, "</head>", 0, true);
        }

        protected int IndexOfBodyEnd(StringBuilder html)
        {
            return this.IndexOf(html, "</body>", 0, true);
        }

        protected int IndexOf(StringBuilder html, string value, int startIndex, bool ignoreCase)
        {
            int length = value.Length;
            int num = html.Length - length + 1;
            if (!ignoreCase)
            {
                for (int index1 = startIndex; index1 < num; ++index1)
                {
                    if ((int)html[index1] == (int)value[0])
                    {
                        int index2 = 1;
                        while (index2 < length && (int)html[index1 + index2] == (int)value[index2])
                            ++index2;
                        if (index2 == length)
                            return index1;
                    }
                }
            }
            else
            {
                for (int index1 = startIndex; index1 < num; ++index1)
                {
                    if ((int)char.ToLower(html[index1]) == (int)char.ToLower(value[0]))
                    {
                        int index2 = 1;
                        while (index2 < length && (int)char.ToLower(html[index1 + index2]) == (int)char.ToLower(value[index2]))
                            ++index2;
                        if (index2 == length)
                            return index1;
                    }
                }
            }
            return -1;
        }
    }

}