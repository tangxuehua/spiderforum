using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.Caching;

namespace System.Web.Core
{
    public class ImgHttpHandler : IHttpHandler
    {
        #region IHttpHandler ≥…‘±

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;

            try
            {
                string imageFile = Globals.MapPath(UrlManager.Instance.GetParameterValue<string>(ParameterName.FileUrl));

                if (File.Exists(imageFile))
                {
                    Image image = Image.FromFile(imageFile);

                    Globals.SetClientCache(context.Response, TimeSpan.FromDays(400), new FileInfo(imageFile).LastWriteTimeUtc);

                    response.ContentType = "image/" + new FileInfo(imageFile).Extension.Substring(1);
                    image.Save(response.OutputStream, image.RawFormat);
                }
            }
            catch { }
        }

        #endregion
    }
}