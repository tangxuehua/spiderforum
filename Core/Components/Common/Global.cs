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
            // �ڻỰ����ʱ���еĴ��롣 
            // ע��: ֻ���� Web.config �ļ��е� sessionstate ģʽ����Ϊ
            // InProc ʱ���Ż����� Session_End �¼�������Ựģʽ����Ϊ StateServer 
            // �� SQLServer���򲻻��������¼���
        }
    }
}
