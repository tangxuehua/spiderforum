using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;

namespace System.Web.Core
{
    public class ImageHttpHandler : IHttpHandler
    {
        #region IHttpHandler ≥…‘±

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;

            response.Clear();
            try
            {
                int picWidth = UrlManager.Instance.GetParameterValue<int>(ParameterName.PictureWidth);
                int picHeight = UrlManager.Instance.GetParameterValue<int>(ParameterName.PictureHeight);
                string imageFile = Globals.MapPath(UrlManager.Instance.GetParameterValue<string>(ParameterName.FileUrl));

                if ((string.IsNullOrEmpty(imageFile) || !File.Exists(imageFile)) && !string.IsNullOrEmpty(Configuration.Instance.NotExistImageDefaultPath))
                {
                    imageFile = Globals.MapPath(Configuration.Instance.NotExistImageDefaultPath);
                }

                if (File.Exists(imageFile))
                {
                    Image image = Globals.GetThumbNailImage(imageFile, picWidth, picHeight);
                    Globals.SetClientCache(context.Response, TimeSpan.FromDays(400), new FileInfo(imageFile).LastWriteTimeUtc);
                    response.ContentType = "image/jpeg";
                    image.Save(response.OutputStream, ImageFormat.Jpeg);
                }
            }
            catch { }
        }

        #endregion
    }
}