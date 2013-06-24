using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.Web.UI;

namespace System.Web.Core
{
    public class AuthCodeHttpHandler : IHttpHandler
    {
        private int intLength = 4;               //长度
        private string cookieKey = "AuthCode";   //随机字串存储键值

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //设置输出流图片格式
                context.Response.ContentType = "image/gif";

                Bitmap b = new Bitmap(70, 22);
                Graphics g = Graphics.FromImage(b);
                g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#EDF2F7")), 0, 0, 70, 22);
                Font font = new Font(FontFamily.GenericSerif, 22, FontStyle.Regular, GraphicsUnit.Pixel);
                Random r = new Random();

                //合法随机显示字符列表
                string strLetters = "1234567890";
                StringBuilder s = new StringBuilder();

                //将随机生成的字符串绘制到图片上
                for (int i = 0; i < intLength; i++)
                {
                    s.Append(strLetters.Substring(r.Next(0, strLetters.Length - 1), 1));
                    g.DrawString(s[s.Length - 1].ToString(), font, new SolidBrush(ColorTranslator.FromHtml("#2265b2")), 4 + i * 14, r.Next(0, 1));
                }

                b.Save(context.Response.OutputStream, ImageFormat.Gif);

                CookieManager.SaveCookie(cookieKey, s.ToString(), 1);
                context.Response.End();
            }
            catch { }
        }

        #endregion
    }
}