using System.Text.RegularExpressions;
using System.Web;

namespace System.Web.Core
{
    public class UrlReWriter
    {
        #region Private Members

        private static UrlReWriter instance;
        private static Regex reWriteFilter;

        #endregion

        #region Singleton Instance

        static UrlReWriter()
        {
            reWriteFilter = new Regex(UrlManager.Instance.LocationFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        private UrlReWriter()
        {
        }
        public static UrlReWriter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UrlReWriter();
                }
                return instance;
            }
        }

        #endregion

        #region Public Methods

        public string GetRewrittenUrl(string path, string queryString)
        {
            string newPath = null;
            if(!reWriteFilter.IsMatch(path))
            {
                Location location = UrlManager.Instance.Locations.FindLocationByPath(path);
                if(location != null)
                {
                    newPath = location.ReWriteUrl(path, queryString);
                }
            }
            return newPath;
        }
        public void RewriteUrl(HttpContext context, string newPath, string queryString)
        {
            context.RewritePath(newPath, null, queryString);
        }

        #endregion
    }
}