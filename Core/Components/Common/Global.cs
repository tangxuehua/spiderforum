using System;
using System.Web;
using System.IO;
using System.Web.UI;
using System.IO.Compression;

namespace System.Web.Core
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            JobManager.Instance.StartAllJobs();
        }

        private void Application_End(object sender, EventArgs e)
        {
            JobManager.Instance.StopAllJobs();
        }

        private void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Globals.ProcessException(true);
            }
            catch
            { }
        }

        private void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            //Only compress the aspx page.
            if (app.Context.CurrentHandler is Page)
            {
                string acceptEncoding = app.Request.Headers["Accept-Encoding"];
                Stream uncompressedStream = app.Response.Filter;

                if (app.Request["HTTP_X_MICROSOFTAJAX"] != null)
                {
                    return;
                }
                if (acceptEncoding == null || acceptEncoding.Length == 0)
                {
                    return;
                }

                acceptEncoding = acceptEncoding.ToLower();

                if (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("x-gzip") || acceptEncoding.Contains("*"))
                {
                    app.Response.Filter = new GZipStream(uncompressedStream, CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", "gzip");
                }
                else if (acceptEncoding.Contains("deflate"))
                {
                    app.Response.Filter = new DeflateStream(uncompressedStream, CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", "deflate");
                }
                app.Context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
            }
        }

        private void Session_Start(object sender, EventArgs e)
        {

        }

        private void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。
        }
    }
}
