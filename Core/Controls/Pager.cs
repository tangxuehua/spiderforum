using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core 
{
    public class Pager : Control 
    {
        #region Private Members

        private string pageHref = "<a href={1}>{0}</a>";
        private string seperatorSpace = "  ";

        #endregion

        #region Constructors

        public Pager()
        {
            this.EnableViewState = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// This property indicates the page index. The minium value is 1.
        /// </summary>
        public int PageIndex
        {
            get
            {
                int pageIndex = 1;
                int? nullableIntegerValue = Globals.GetNullableIntegerValue(ViewState, "PageIndex");
                if (nullableIntegerValue.HasValue)
                {
                    pageIndex = nullableIntegerValue.Value;
                }
                else
                {
                    pageIndex = UrlManager.Instance.GetParameterValue<int>(ParameterName.PageIndex);
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }
        /// <summary>
        /// This property indicates the page size. The minium value is 1.
        /// </summary>
        public int PageSize
        {
            get
            {
                int pageSize = 20;  //Set the default value.
                int? nullableIntegerValue = Globals.GetNullableIntegerValue(ViewState, "PageSize");
                if (nullableIntegerValue.HasValue)
                {
                    pageSize = nullableIntegerValue.Value;
                }
                if (pageSize < 1)
                {
                    pageSize = 1;
                }
                return pageSize;
            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }
        /// <summary>
        /// This property indicates the length of the pager. That means how many page numbers the pager should display.
        /// E.g. PageLength = 7 means the pager can most display 7 page numbers. 1,2,3,4,5,6,7 or 30,31,32,33,34,45,36
        /// Notes: The PageLength should be an Odd number, like 1,3,5,7,9,11,etc.
        /// The default value is 5, and the minimum value is 3.
        /// </summary>
        public int PageLength
        {
            get
            {
                int pageLength = 5;  //Set the default value.
                int? nullableIntegerValue = Globals.GetNullableIntegerValue(ViewState, "PageLength");
                if (nullableIntegerValue.HasValue)
                {
                    pageLength = nullableIntegerValue.Value;
                }
                if (pageLength < 3)
                {
                    return 3;
                }
                if (pageLength % 2 == 0)
                {
                    return pageLength + 1;
                }
                return pageLength;
            }
            set
            {
                ViewState["PageLength"] = value;
            }
        }
        /// <summary>
        /// This property indicates the total page number.
        /// </summary>
        public int TotalPages
        {
            get
            {
                return Globals.CalculateTotalPages(TotalRecords, PageSize);
            }
        }
        /// <summary>
        /// This property indicates the total record count.
        /// </summary>
        public int TotalRecords
        {
            get
            {
                int totalRecords = 0;
                int? nullableIntegerValue = Globals.GetNullableIntegerValue(ViewState, "TotalRecords");
                if (nullableIntegerValue.HasValue)
                {
                    totalRecords = nullableIntegerValue.Value;
                }
                return totalRecords;
            }
            set
            {
                ViewState["TotalRecords"] = value;
            }
        }
        /// <summary>
        /// This property indicates a url name which is managed in Urls.xml config.
        /// </summary>
        public string UrlName
        {
            get
            {
                return Globals.GetStringValue(ViewState, "UrlName");
            }
            set
            {
                ViewState["UrlName"] = value;
            }
        }
        /// <summary>
        /// This property indicates a regex pattern of url.
        /// </summary>
        public string UrlPattern
        {
            get
            {
                return Globals.GetStringValue(ViewState, "UrlPattern");
            }
            set
            {
                ViewState["UrlPattern"] = value;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Overrides this function to render all the paging items.
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            if (TotalPages <= 1)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            //append the first page.
            if ((PageIndex > (PageLength / 2 + 1)) && (TotalPages > PageLength))
            {
                sb.Append(CreatePageHref("第一页", 1));
                sb.Append(seperatorSpace);
            }

            //append the previous page.
            if (PageIndex > 1)
            {
                sb.Append(CreatePageHref("上一页", PageIndex - 1));
                sb.Append(seperatorSpace);
            }

            //append the number pages.
            sb.Append(GetNumberPages());
            sb.Append(seperatorSpace);

            //append the next page.
            if (PageIndex < TotalPages)
            {
                sb.Append(CreatePageHref("下一页", PageIndex + 1));
                sb.Append(seperatorSpace);
            }

            //append the last page.
            if (((PageIndex + PageLength / 2) < TotalPages) && (TotalPages > PageLength))
            {
                sb.Append(CreatePageHref("最后一页", TotalPages));
            }

            //write the total contents.
            writer.Write(sb.ToString());
        }
        /// <summary>
        /// This function used to create the href attribute value of the A html tag.
        /// </summary>
        protected virtual string CreateUrl(string pageIndex)
        {
            if (!string.IsNullOrEmpty(UrlPattern))
            {
                return string.Format(UrlPattern, pageIndex);
            }
            if (!string.IsNullOrEmpty(UrlName))
            {
                return UrlManager.Instance.FormatUrl(UrlName, new object[] { pageIndex });
            }

            string pageIndexParameterName = ParameterName.PageIndex.ToString();
            string rawUrl = HttpContext.Current.Request.RawUrl;

            if (rawUrl.IndexOf("?") == -1)
            {
                return rawUrl + "?" + pageIndexParameterName + "=" + pageIndex;
            }
            else
            {
                if (rawUrl.IndexOf(pageIndexParameterName + "=", StringComparison.CurrentCultureIgnoreCase) == -1)
                {
                    return rawUrl + "&" + pageIndexParameterName + "=" + pageIndex;
                }
                else
                {
                    return Regex.Replace(rawUrl, @"PageIndex=(\d+\.?\d*|\.\d+)", pageIndexParameterName + "=" + pageIndex, RegexOptions.IgnoreCase);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function used to create the text and href attribute value of the A html tag.
        /// </summary>
        private string CreatePageHref(string text, int pageIndex)
        {
            return string.Format(pageHref, text, CreateUrl(pageIndex.ToString()));
        }
        /// <summary>
        /// This function used to render all the paging numbers. Like: 1,2,3,4,5
        /// </summary>
        private string GetNumberPages()
        {
            int totalPages = TotalPages;
            int pageIndex = PageIndex;
            int pageLength = PageLength;

            //First, calculate the startIndex and endIndex.
            int startIndex = pageIndex - pageLength / 2;
            int endIndex = pageIndex + pageLength / 2;

            if (startIndex < 1)
            {
                endIndex += 1 - startIndex;
                startIndex = 1;
            }
            if (endIndex > totalPages)
            {
                startIndex -= endIndex - totalPages;
                endIndex = totalPages;
            }
            if (startIndex < 1)
            {
                startIndex = 1;
            }
            if (endIndex > totalPages)
            {
                endIndex = totalPages;
            }

            //Second, render all the paging numbers.
            StringBuilder sb = new StringBuilder();
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (pageIndex == i)
                {
                    sb.Append("[" + i.ToString() + "]");
                }
                else
                {
                    sb.Append(CreatePageHref("[" + i.ToString() + "]", i));
                }
                if (i < endIndex)
                {
                    sb.Append(seperatorSpace);
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}