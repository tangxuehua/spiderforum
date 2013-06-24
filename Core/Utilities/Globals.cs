using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Caching;

namespace System.Web.Core
{
    public class Globals
    {
        private static string applicationPath = null;
        private static string applicationPhysicalPath = null;

        /// <summary>
        /// 返回当前Web应用程序的根虚拟目录，结尾不带斜杠/。
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                if (applicationPath == null)
                {
                    applicationPath = VirtualPathUtility.ToAbsolute("~").TrimEnd('/');
                }
                return applicationPath;
            }
        }

        /// <summary>
        /// 返回当前Web应用程序的根物理目录，结尾不带斜杠\。
        /// </summary>
        public static string ApplicationPhysicalPath
        {
            get
            {
                if (applicationPhysicalPath == null)
                {
                    applicationPhysicalPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", Path.DirectorySeparatorChar.ToString()).TrimEnd(Path.DirectorySeparatorChar);
                }
                return applicationPhysicalPath;
            }
        }

        /// <summary>
        /// 将虚拟目录映射为物理目录。
        /// </summary>
        public static string MapPath(string virtualPath)
        {
            return string.Concat
            (
                ApplicationPhysicalPath.TrimEnd(Path.DirectorySeparatorChar),
                Path.DirectorySeparatorChar.ToString(),
                virtualPath.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", "").TrimStart(Path.DirectorySeparatorChar)
            );
        }

        public static void Return404(HttpContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.SuppressContent = true;
            context.Response.End();
        }

        public static string HtmlEncode(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("&", "&amp;");
                text = text.Replace("<", "&lt;");
                text = text.Replace(">", "&gt;");
                text = text.Replace(" ", "&nbsp;");
                text = text.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                text = text.Replace("'", "&#39;");
                text = text.Replace("\"", "&quot;");
                text = text.Replace(Environment.NewLine, "<br>");
                text = text.Replace("\n", "<br>");
            }
            return text;
        }

        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
                return urlToEncode;

            return System.Web.HttpUtility.UrlEncode(urlToEncode).Replace("'", "%27");
        }
        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
                return urlToDecode;

            return System.Web.HttpUtility.UrlDecode(urlToDecode);
        }

        /// <summary>
        /// 将一个对象转换为另外一个给定的类型
        /// </summary>
        public static T ChangeType<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));

            try
            {
                if (tc.CanConvertFrom(value.GetType()))
                {
                    return (T)tc.ConvertFrom(value);
                }
                else
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static T ConvertType<T>(object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));

            if (tc.CanConvertFrom(value.GetType()))
            {
                return (T)tc.ConvertFrom(value);
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

        public static TParentControl FindParent<TParentControl>(Control control) where TParentControl : Control
        {
            if (control != null)
            {
                TParentControl parent = control.Parent as TParentControl;
                if (parent != null)
                {
                    return parent;
                }
                TParentControl ancester = FindParent<TParentControl>(control.Parent);
                if (ancester != null)
                {
                    return ancester;
                }
            }
            return null;
        }
        public static Control FindControl(Control container, string id)
        {
            if ((container == null) || string.IsNullOrEmpty(id))
            {
                return null;
            }
            if (!(container is INamingContainer) && (container.NamingContainer != null))
            {
                container = container.NamingContainer;
            }
            Control control = FindControlInternal(container, id, null);
            if (control == null)
            {
                Dictionary<Control, bool> dictionary = new Dictionary<Control, bool>();
                dictionary[container] = true;
                container = container.NamingContainer;
                while ((container != null) && (control == null))
                {
                    control = FindControlInternal(container, id, dictionary);
                    dictionary[container] = true;
                    container = container.NamingContainer;
                }
            }
            return control;
        }
        private static Control FindControlInternal(Control control1, string text1, Dictionary<Control, bool> dictionary1)
        {
            if (control1.ID == text1)
            {
                return control1;
            }
            Control control = control1.FindControl(text1);
            if ((control != null) && (control.ID == text1))
            {
                return control;
            }
            foreach (Control control2 in control1.Controls)
            {
                if ((dictionary1 == null) || !dictionary1.ContainsKey(control2))
                {
                    control = FindControlInternal(control2, text1, dictionary1);
                    if (control != null)
                    {
                        return control;
                    }
                }
            }
            return null;
        }

        public static string RenderControl(Control control)
        {
            StringWriter writer1 = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer1);

            control.RenderControl(writer2);
            writer2.Flush();
            writer2.Close();

            return writer1.ToString();
        }
        public static void ShowMessage(Page page, string strKey, string strInfo)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered(strKey))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), strKey, "alert('" + strInfo + "');history.go(-1);", true);
            }
        }

        public static string GetControlValue(Control control)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request[control.UniqueID]))
                {
                    return request[control.UniqueID];
                }
                return GetCompositeValueWithPrefix(request, control.UniqueID);
            }
            return null;
        }
        private static string GetCompositeValueWithPrefix(HttpRequest request, string prefix)
        {
            List<string> valueList = new List<string>();
            foreach (string key in request.Form.AllKeys)
            {
                if (key.IndexOf(prefix) >= 0)
                {
                    valueList.Add(request.Form[key]);
                }
            }
            if (valueList.Count > 0)
            {
                return string.Join(",", valueList.ToArray());
            }
            return null;
        }

        /// <summary>
        /// This function used to calculate the total pages.
        /// </summary>
        public static int CalculateTotalPages(int totalRecords, int pageSize)
        {
            int totalPages;

            if (totalRecords == 0)
            {
                return 0;
            }

            totalPages = totalRecords / pageSize;

            if (totalRecords % pageSize > 0)
            {
                totalPages++;
            }

            return totalPages;
        }

        /// <summary>
        /// This function used to return a nullable integer value from the ViewState with the specified view state key.
        /// </summary>
        public static int? GetNullableIntegerValue(StateBag viewState, string viewStateKey)
        {
            int? value = null;
            if (!string.IsNullOrEmpty(viewStateKey) && viewState != null && viewState[viewStateKey] != null)
            {
                int tempValue = 0;
                if (int.TryParse(viewState[viewStateKey].ToString(), out tempValue))
                {
                    value = tempValue;
                }
            }
            return value;
        }

        /// <summary>
        /// This function used to return a string value from the ViewState with the specified view state key.
        /// </summary>
        public static string GetStringValue(StateBag viewState, string viewStateKey)
        {
            string value = null;
            if (!string.IsNullOrEmpty(viewStateKey) && viewState != null && viewState[viewStateKey] != null)
            {
                value = viewState[viewStateKey].ToString();
            }
            return value;
        }
        /// <summary>
        /// When we use a customize stored procedure and with a 3 three top algorithm to get a page of records, if the current page is the last page,
        /// then we need to fix the 3 three top algorithm last page bug to remove the unnecessary records.
        /// </summary>
        public static EntityList FixLastPageBug(int pageIndex, int pageSize, int totalRecords, EntityList originalEntityList)
        {
            EntityList fixedEntityList = originalEntityList;
            if (pageIndex < 1 || pageIndex > Globals.CalculateTotalPages(totalRecords, pageSize))
            {
                fixedEntityList = new EntityList();
            }
            else if (totalRecords > pageSize)
            {
                if (pageIndex * pageSize > totalRecords && (pageIndex - 1) * pageSize < totalRecords)
                {
                    EntityList modifiedEntityList = new EntityList();
                    for (int i = pageIndex * pageSize - totalRecords; i < originalEntityList.Count; i++)
                    {
                        modifiedEntityList.Add(originalEntityList[i]);
                    }
                    fixedEntityList = modifiedEntityList;
                }
            }
            return fixedEntityList;
        }
        /// <summary>
        /// This function get the current page entities from a total entity list with the given pageIndex and pageSize.
        /// </summary>
        public static EntityList GetCurrentPageEntities(EntityList totalEntityList, int pageIndex, int pageSize)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;

            EntityList currentPageEntityList = new EntityList();
            for (int index = startIndex; index < endIndex && index >= 0 && index < totalEntityList.Count; index++)
            {
                currentPageEntityList.Add(totalEntityList[index]);
            }

            return currentPageEntityList;
        }

        public static string GetFileNewUniqueName()
        {
            return Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
        }

        public static byte[] GetBytesFromStream(Stream stream)
        {
            byte[] content = new Byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(content, 0, (int)stream.Length);
            return content;
        }

        public static string GetFileStorageUrl(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return ConfigurationManager.AppSettings["StorageFolder"].Replace("~", string.Empty) + @"/" + fileName;
            }
            return null;
        }
        public static string GetFileStoragePath(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return MapPath(ConfigurationManager.AppSettings["StorageFolder"].Replace("~", string.Empty) + @"/" + fileName);
            }
            return null;
        }
        public static void SaveFile(FileUpload fileUpload, string fileName)
        {
            string fileFullPath = GetFileStoragePath(fileName);
            if (!IsAttachmentValid(fileUpload) || string.IsNullOrEmpty(fileFullPath))
            {
                return;
            }
            fileUpload.PostedFile.SaveAs(fileFullPath);
        }
        public static void DeleteFile(AttachmentEntity attachmentEntity)
        {
            DeleteFile(attachmentEntity.AttachmentFileName.Value);
        }
        public static void DeleteFile(string fileName)
        {
            string fileFullPath = GetFileStoragePath(fileName);

            if (!string.IsNullOrEmpty(fileFullPath) && File.Exists(fileFullPath))
            {
                try
                {
                    FileInfo fi = new FileInfo(fileFullPath);
                    if (((fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) || ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
                    {
                        fi.Attributes = FileAttributes.Normal;
                    }
                    //File.Delete(fileFullPath);  //TODO, 暂时步删除物理磁盘上的文件
                }
                catch { }
            }
        }

        /// <summary>
        /// 获取一个图片按等比例缩小后的大小。
        /// </summary>
        /// <param name="maxWidth">需要缩小到的宽度</param>
        /// <param name="maxHeight">需要缩小到的高度</param>
        /// <param name="imageOriginalWidth">图片的原始宽度</param>
        /// <param name="imageOriginalHeight">图片的原始高度</param>
        /// <returns>返回图片按等比例缩小后的实际大小</returns>
        public static Size GetNewSize(int maxWidth, int maxHeight, int imageOriginalWidth, int imageOriginalHeight)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(imageOriginalWidth);
            double sh = Convert.ToDouble(imageOriginalHeight);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);

            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }

            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        /// <summary>
        /// 对给定的一个图片（Image对象）生成一个指定大小的缩略图。
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <param name="thumMaxWidth">缩略图的宽度</param>
        /// <param name="thumMaxHeight">缩略图的高度</param>
        /// <returns>返回缩略图的Image对象</returns>
        public static System.Drawing.Image GetThumbNailImage(System.Drawing.Image originalImage, int thumMaxWidth, int thumMaxHeight)
        {
            Size thumRealSize = Size.Empty;
            System.Drawing.Image newImage = originalImage;
            Graphics graphics = null;

            try
            {
                thumRealSize = GetNewSize(thumMaxWidth, thumMaxHeight, originalImage.Width, originalImage.Height);
                newImage = new Bitmap(thumRealSize.Width, thumRealSize.Height);
                graphics = Graphics.FromImage(newImage);

                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.Clear(Color.Transparent);

                graphics.DrawImage(originalImage, new Rectangle(0, 0, thumRealSize.Width, thumRealSize.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
            }
            catch { }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                    graphics = null;
                }
            }

            return newImage;
        }
        /// <summary>
        /// 对给定的一个图片文件生成一个指定大小的缩略图。
        /// </summary>
        /// <param name="originalImage">图片的物理文件地址</param>
        /// <param name="thumMaxWidth">缩略图的宽度</param>
        /// <param name="thumMaxHeight">缩略图的高度</param>
        /// <returns>返回缩略图的Image对象</returns>
        public static System.Drawing.Image GetThumbNailImage(string imageFile, int thumMaxWidth, int thumMaxHeight)
        {
            System.Drawing.Image originalImage = null;
            System.Drawing.Image newImage = null;

            try
            {
                originalImage = System.Drawing.Image.FromFile(imageFile);
                newImage = GetThumbNailImage(originalImage, thumMaxWidth, thumMaxHeight);
            }
            catch { }
            finally
            {
                if (originalImage != null)
                {
                    originalImage.Dispose();
                    originalImage = null;
                }
            }

            return newImage;
        }
        /// <summary>
        /// 对给定的一个图片文件生成一个指定大小的缩略图，并将缩略图保存到指定位置。
        /// </summary>
        /// <param name="originalImageFile">图片的物理文件地址</param>
        /// <param name="thumbNailImageFile">缩略图的物理文件地址</param>
        /// <param name="thumMaxWidth">缩略图的宽度</param>
        /// <param name="thumMaxHeight">缩略图的高度</param>
        public static void MakeThumbNail(string originalImageFile, string thumbNailImageFile, int thumMaxWidth, int thumMaxHeight)
        {
            System.Drawing.Image newImage = GetThumbNailImage(originalImageFile, thumMaxWidth, thumMaxHeight);
            try
            {
                newImage.Save(thumbNailImageFile, ImageFormat.Jpeg);
            }
            catch
            { }
            finally
            {
                newImage.Dispose();
                newImage = null;
            }
        }
        /// <summary>
        /// 将一个图片的内存流调整为指定大小，并返回调整后的内存流。
        /// </summary>
        /// <param name="originalImageStream">原始图片的内存流</param>
        /// <param name="newWidth">新图片的宽度</param>
        /// <param name="newHeight">新图片的高度</param>
        /// <returns>返回调整后的图片的内存流</returns>
        public static MemoryStream ResizeImage(Stream originalImageStream, int newWidth, int newHeight)
        {
            MemoryStream newImageStream = null;

            System.Drawing.Image newImage = Globals.GetThumbNailImage(System.Drawing.Image.FromStream(originalImageStream), newWidth, newHeight);
            if (newImage != null)
            {
                newImageStream = new MemoryStream();
                newImage.Save(newImageStream, ImageFormat.Jpeg);
            }

            return newImageStream;
        }
        /// <summary>
        /// 将一个内存流保存为磁盘文件。
        /// </summary>
        /// <param name="stream">内存流</param>
        /// <param name="newFile">目标磁盘文件地址</param>
        public static void SaveStreamToFile(Stream stream, string newFile)
        {
            if (stream == null || stream.Length == 0 || string.IsNullOrEmpty(newFile))
            {
                return;
            }

            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            FileStream fileStream = new FileStream(newFile, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }
        /// <summary>
        /// 对一个指定的图片加上图片水印效果。
        /// </summary>
        /// <param name="imageFile">图片文件地址</param>
        /// <param name="waterImage">水印图片（Image对象）</param>
        public static void CreateImageWaterMark(string imageFile, System.Drawing.Image waterImage)
        {
            if (string.IsNullOrEmpty(imageFile) || !File.Exists(imageFile) || waterImage == null)
            {
                return;
            }

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(imageFile);

            if (originalImage.Width - 10 < waterImage.Width || originalImage.Height - 10 < waterImage.Height)
            {
                return;
            }

            Graphics graphics = Graphics.FromImage(originalImage);

            int x = originalImage.Width - waterImage.Width - 10;
            int y = originalImage.Height - waterImage.Height - 10;
            int width = waterImage.Width;
            int height = waterImage.Height;

            graphics.DrawImage(waterImage, new Rectangle(x, y, width, height), 0, 0, width, height, GraphicsUnit.Pixel);
            graphics.Dispose();

            MemoryStream stream = new MemoryStream();
            originalImage.Save(stream, ImageFormat.Jpeg);
            originalImage.Dispose();

            System.Drawing.Image imageWithWater = System.Drawing.Image.FromStream(stream);

            imageWithWater.Save(imageFile);
            imageWithWater.Dispose();
        }
        /// <summary>
        /// 对一个指定的图片加上文字水印效果。
        /// </summary>
        /// <param name="imageFile">图片文件地址</param>
        /// <param name="waterText">水印文字内容</param>
        public static void CreateTextWaterMark(string imageFile, string waterText)
        {
            if (string.IsNullOrEmpty(imageFile) || string.IsNullOrEmpty(waterText) || !File.Exists(imageFile))
            {
                return;
            }

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(imageFile);

            Graphics graphics = Graphics.FromImage(originalImage);

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            SolidBrush brush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            Font waterTextFont = new Font("Arial", 16, FontStyle.Regular);
            SizeF waterTextSize = graphics.MeasureString(waterText, waterTextFont);

            float x = (float)originalImage.Width - waterTextSize.Width - 10F;
            float y = (float)originalImage.Height - waterTextSize.Height - 10F;

            graphics.DrawString(waterText, waterTextFont, brush, x, y);

            graphics.Dispose();
            brush.Dispose();

            MemoryStream stream = new MemoryStream();
            originalImage.Save(stream, ImageFormat.Jpeg);
            originalImage.Dispose();

            System.Drawing.Image imageWithWater = System.Drawing.Image.FromStream(stream);

            imageWithWater.Save(imageFile);
            imageWithWater.Dispose();
        }

        /// <summary>
        /// 判断上传组件是否包含内容。
        /// </summary>
        /// <param name="fileUpload">ASP.NET 2.0标准上传组件</param>
        /// <returns>如果数据有效，则返回True，否则返回False</returns>
        public static bool IsAttachmentValid(FileUpload fileUpload)
        {
            if (fileUpload != null &&
                fileUpload.PostedFile != null &&
                !string.IsNullOrEmpty(fileUpload.PostedFile.FileName) &&
                fileUpload.PostedFile.ContentLength > 0)
            {
                return true;
            }
            return false;
        }
        public static void SaveImageWithImageWatermark(FileUpload fileUpload, string fileName, System.Drawing.Image waterImage)
        {
            string fileFullPath = GetFileStoragePath(fileName);
            if (!IsAttachmentValid(fileUpload) || string.IsNullOrEmpty(fileFullPath))
            {
                return;
            }
            fileUpload.PostedFile.SaveAs(fileFullPath);
            if (waterImage != null)
            {
                CreateImageWaterMark(fileFullPath, waterImage);
            }
        }
        public static void SaveImageWithImageWatermark(FileUpload fileUpload, string fileName, string watermarkImageFile)
        {
            if (!string.IsNullOrEmpty(watermarkImageFile) && File.Exists(watermarkImageFile))
            {
                SaveImageWithImageWatermark(fileUpload, fileName, System.Drawing.Image.FromFile(watermarkImageFile));
            }
            else
            {
                SaveFile(fileUpload, fileName);
            }
        }
        public static void SaveImageWithTextWatermark(FileUpload fileUpload, string fileName, string waterText)
        {
            string fileFullPath = GetFileStoragePath(fileName);
            if (!IsAttachmentValid(fileUpload) || string.IsNullOrEmpty(fileFullPath))
            {
                return;
            }
            fileUpload.PostedFile.SaveAs(fileFullPath);
            if (!string.IsNullOrEmpty(waterText))
            {
                CreateTextWaterMark(fileFullPath, waterText);
            }
        }

        public static void ProcessException()
        {
            ProcessException(false);
        }
        public static void ProcessException(bool redirectToErrorPage)
        {
            ExceptionLog exceptinLog = null;
            HttpServerUtility server = HttpContext.Current.Server;

            Exception lastException = server.GetLastError();
            if (lastException != null)
            {
                server.ClearError();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                Exception lastRealException = lastException.GetBaseException();
                if (lastRealException != null)
                {
                    exceptinLog = Globals.LogException(lastRealException);
                    if (redirectToErrorPage && HttpContext.Current.Request.Url.LocalPath.ToLower().EndsWith(".aspx") && exceptinLog != null)
                    {
                        if (!File.Exists(HttpContext.Current.Request.PhysicalPath))
                        {
                            HttpContext.Current.Response.Write(string.Format("<script type=\"text/javascript\">window.top.location=\"{0}\";</script>", Globals.ApplicationPath + Configuration.Instance.NotFoundPage));
                        }
                        else
                        {
                            HttpContext.Current.Response.Write(string.Format("<script type=\"text/javascript\">window.top.location=\"{0}\";</script>", Globals.ApplicationPath + Configuration.Instance.SiteErrorPage));
                        }
                    }
                }
            }
        }
        public static ExceptionLog LogException(Exception exception)
        {
            ExceptionLog exceptionLog = null;
            HttpRequest request = HttpContext.Current.Request;

            TRequest<ExceptionLog> exceptionLogRequest = new TRequest<ExceptionLog>();
            exceptionLogRequest.Data.PathAndQuery.Value = request.Url != null ? request.Url.PathAndQuery : null;
            exceptionLogRequest.Data.HttpVerb.Value = request.RequestType;
            exceptionLogRequest.Data.Message.Value = exception.Message;
            exceptionLogRequest.Data.ExceptionDetails.Value = exception.ToString();
            exceptionLog = Engine.GetSingle<ExceptionLog>(exceptionLogRequest);

            if (exceptionLog != null)
            {
                exceptionLog.DateLastOccurred.Value = DateTime.Now;
                exceptionLog.IPAddress.Value = request.UserHostAddress;
                exceptionLog.HttpReferrer.Value = request.UrlReferrer != null ? request.UrlReferrer.ToString() : null;
                exceptionLog.UserAgent.Value = request.UserAgent;
                exceptionLog.Frequency.Value = exceptionLog.Frequency.Value + 1;
                exceptionLog.UserName.Value = MemberManager.GetCurrentLoggedOnMemberName();
                Engine.Update(exceptionLog);
            }
            else
            {
                exceptionLog = CreateNewExceptionLog(exception);
            }

            return exceptionLog;
        }
        public static ExceptionLog CreateNewExceptionLog(Exception exception)
        {
            ExceptionLog exceptionLog = new ExceptionLog();

            HttpRequest request = HttpContext.Current.Request;

            if (request != null)
            {
                exceptionLog.HttpReferrer.Value = request.UrlReferrer != null ? request.UrlReferrer.ToString() : null;
                exceptionLog.UserAgent.Value = request.UserAgent;
                exceptionLog.IPAddress.Value = request.UserHostAddress;
                exceptionLog.HttpVerb.Value = request.RequestType;
                exceptionLog.PathAndQuery.Value = request.Url != null ? request.Url.PathAndQuery : null;
            }

            exceptionLog.Message.Value = exception.Message;
            exceptionLog.ExceptionDetails.Value = exception.ToString();
            exceptionLog.DateCreated.Value = DateTime.Now;
            exceptionLog.DateLastOccurred.Value = exceptionLog.DateCreated.Value;
            exceptionLog.Frequency.Value = 1;
            exceptionLog.UserName.Value = MemberManager.GetCurrentLoggedOnMemberName();

            Engine.Create(exceptionLog);

            return exceptionLog;
        }

        public static void SetClientCache(HttpResponse response, TimeSpan expiresDate)
        {
            SetClientCache(response, expiresDate, null);
        }
        public static void SetClientCache(HttpResponse response, TimeSpan expiresDate, DateTime? lastModifiedDate)
        {
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetExpires(DateTime.Now.Add(expiresDate));
            response.Cache.SetMaxAge(expiresDate);
            response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            if (lastModifiedDate != null)
            {
                response.Cache.SetLastModified(lastModifiedDate.Value);
            }
        }
    }
}
