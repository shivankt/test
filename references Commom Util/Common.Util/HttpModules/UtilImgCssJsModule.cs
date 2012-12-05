using System;
using System.IO.Compression;
using System.Web;

namespace Common.Util.HttpModules
{
    public class UtilImgCssJsModule : IHttpModule
    {
        public string ModuleName
        {
            get
            {
                return "UtilCssJsCompressionModule";
            }
        }

        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app.Request.RawUrl.Contains(".css")
                || app.Request.RawUrl.Contains(".js")
                )
            {
                string acceptEncoding = string.Empty;
                if (!IsEncodingSupported(ref app, ref acceptEncoding))
                    return;
                else
                    AddExpireHeader(ZipResponse(app.Response, acceptEncoding));
            }
            else if (app.Request.RawUrl.Contains(".jpg") 
                    || app.Request.RawUrl.Contains(".png")
                    || app.Request.RawUrl.Contains(".gif")
                    || app.Request.RawUrl.Contains(".ico")
                    || app.Request.RawUrl.Contains("WebResource.axd")
                )
            {
                AddExpireHeader(app.Response);
            }
        }

        HttpResponse ZipResponse(HttpResponse response, string acceptEncoding)
        {
            acceptEncoding = acceptEncoding.ToUpperInvariant();

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress, true);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress, true);
            }
            return response;
        }

        void AddExpireHeader(HttpResponse response)
        {
            //http://weblogs.asp.net/rashid/archive/2008/03/28/asp-net-mvc-action-filter-caching-and-compression.aspx
            //http://msdn.microsoft.com/en-us/library/system.web.httpcachepolicy.appendcacheextension.aspx

            TimeSpan cacheDuration = new TimeSpan(30, 0, 0, 0);

            response.Cache.SetExpires(DateTime.Now.Add(cacheDuration));
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetNoServerCaching();
            response.Cache.SetMaxAge(cacheDuration);
            response.Cache.AppendCacheExtension("post-check=900,pre-check=3600");

        }

        bool IsEncodingSupported(ref HttpApplication app, ref string acceptEncoding)
        {
            acceptEncoding = app.Context.Request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(acceptEncoding)
                && (acceptEncoding.ToUpper().Contains("GZIP") || acceptEncoding.ToUpper().Contains("DEFLATE"))
                )
            {
                acceptEncoding = acceptEncoding.ToUpper();
                return true;
            }

            return false;
        }
    }
}
