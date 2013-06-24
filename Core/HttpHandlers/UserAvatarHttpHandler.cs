using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;

namespace System.Web.Core
{
    public class UserAvatarHttpHandler : IHttpHandler
    {
        #region IHttpHandler ≥…‘±

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            Image image = null;

            int userId = UrlManager.Instance.GetParameterValue<int>(ParameterName.UserId);
            int picWidth = UrlManager.Instance.GetParameterValue<int>(ParameterName.PictureWidth);
            int picHeight = UrlManager.Instance.GetParameterValue<int>(ParameterName.PictureHeight);

            response.Clear();
            try
            {
                User user = UserManager.GetUser(userId);
                if (user != null && user.AvatarContent.Value != null && user.AvatarContent.Value.Length > 0)
                {
                    image = Globals.GetThumbNailImage(Image.FromStream(new MemoryStream(user.AvatarContent.Value)), picWidth, picHeight);
                }
                else if (!string.IsNullOrEmpty(Configuration.Instance.UserDefaultAvatarPath))
                {
                    string defaultAvatarFile = Globals.MapPath(Configuration.Instance.UserDefaultAvatarPath);
                    if (File.Exists(defaultAvatarFile))
                    {
                        image = Globals.GetThumbNailImage(defaultAvatarFile, picWidth, picHeight);
                    }
                }
                if (image != null)
                {
                    Globals.SetClientCache(context.Response, TimeSpan.FromDays(400));
                    response.ContentType = "image/jpeg";
                    image.Save(response.OutputStream, ImageFormat.Jpeg);
                }
            }
            catch { }
        }

        #endregion
    }
}