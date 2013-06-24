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
        private int intLength = 4;               //����
        private string cookieKey = "AuthCode";   //����ִ��洢��ֵ

        #region IHttpHandler ��Ա

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //���������ͼƬ��ʽ
                context.Response.ContentType = "image/gif";

                Bitmap b = new Bitmap(70, 22);
                Graphics g = Graphics.FromImage(b);
                g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#EDF2F7")), 0, 0, 70, 22);
                Font font = new Font(FontFamily.GenericSerif, 22, FontStyle.Regular, GraphicsUnit.Pixel);
                Random r = new Random();

                //�Ϸ������ʾ�ַ��б�
                string strLetters = "1234567890";
                StringBuilder s = new StringBuilder();

                //��������ɵ��ַ������Ƶ�ͼƬ��
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