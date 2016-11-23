using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Diagnostics;
using Sitecore.Modules.Framework.MVC.Text;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Modules.Framework.MVC.IO
{
    public class HtmlUpdateFilter : Stream
    {
        protected readonly MemoryStream InternalStream;
        protected readonly IHtmlUpdater[] Updaters;

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return 0L;
            }
        }

        public override long Position { get; set; }

        public Stream ResponseStream { get; protected set; }

        public virtual Encoding Encoding
        {
            get
            {
                PageContext currentOrNull = PageContext.CurrentOrNull;
                if (currentOrNull == null)
                    return Encoding.UTF8;
                else
                    return currentOrNull.RequestContext.HttpContext.Response.ContentEncoding;
            }
        }

        public HtmlUpdateFilter(Stream stream, IEnumerable<IHtmlUpdater> updaters)
        {
            Assert.ArgumentNotNull((object)stream, "stream");
            Assert.ArgumentNotNull((object)updaters, "updaters");
            this.ResponseStream = stream;
            this.InternalStream = new MemoryStream();
            this.Updaters = Enumerable.ToArray<IHtmlUpdater>(updaters);
        }

        public override void Flush()
        {
            byte[] numArray = this.InternalStream.ToArray();
            StringBuilder builder;
            if (this.Updaters.Length > 0 && this.TryUpdateHtml(this.Encoding.GetString(numArray), out builder))
                numArray = this.Encoding.GetBytes(((object)builder).ToString());
            this.TransmitData(numArray);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.ResponseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.ResponseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            this.ResponseStream.SetLength(length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.InternalStream.Write(buffer, offset, count);
        }

        protected virtual void TransmitData(byte[] data)
        {
            this.ResponseStream.Write(data, 0, data.Length);
            this.ResponseStream.Flush();
            this.InternalStream.SetLength(0L);
            this.Position = 0L;
        }

        protected virtual bool TryUpdateHtml(string html, out StringBuilder builder)
        {
            builder = new StringBuilder(html);
            bool flag = false;
            foreach (IHtmlUpdater htmlUpdater in this.Updaters)
            {
                try
                {
                    flag = htmlUpdater.UpdateHtml(builder) | flag;
                }
                catch (Exception ex)
                {
                    ////LogHelper.Error("Error during injecting html", (object)this, ex);
                }
            }
            return flag;
        }

    }
}
