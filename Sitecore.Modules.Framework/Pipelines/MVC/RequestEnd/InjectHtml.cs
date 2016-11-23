using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Sitecore.Modules.Framework.MVC.IO;
using Sitecore.Modules.Framework.MVC.Text;
using Sitecore.Mvc.Pipelines.Request.RequestEnd;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Modules.Framework.Pipelines.MVC.RequestEnd
{
    public class InjectHtml : RequestEndProcessor
    {
        protected List<IHtmlUpdater> Updaters { get; set; }

        public InjectHtml()
        {
            this.Updaters = new List<IHtmlUpdater>();
        }

        public override void Process(RequestEndArgs args)
        {
            try
            {
                PageContext pageContext = args.PageContext;
                if (pageContext == null)
                    return;
                RequestContext requestContext = pageContext.RequestContext;
                Stream filter = requestContext.HttpContext.Response.Filter;
                if (filter == null || this.Updaters.Count <= 0)
                    return;
                requestContext.HttpContext.Response.Filter = (Stream)new HtmlUpdateFilter(filter, (IEnumerable<IHtmlUpdater>)this.Updaters);
            }
            catch (InvalidOperationException ex)
            {
            }
        }
    }
}