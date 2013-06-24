using System.Text.RegularExpressions;

namespace System.Web.Core
{
    public class ReWrittenUrl
    {
        #region Private Members

        private string name;
        private DynamicPath dynamicPath;
        private string realPath;
        private string pattern;
        private Regex regex;

        #endregion

        #region Constructors

        public ReWrittenUrl(string name, string pattern, DynamicPath dynamicPath, string realPath)
        {
            this.name = name;
            this.dynamicPath = dynamicPath;
            this.realPath = realPath;
            this.pattern = "^" + pattern;
            this.regex = new Regex(this.pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return name; }
        }
        public DynamicPath DynamicPath
        {
            get { return dynamicPath; }
        }
        public string RealPath
        {
            get { return realPath; }
        }
        public string Pattern
        {
            get { return pattern; }
        }

        #endregion

        #region Public Methods

        public bool IsMatch(string url)
        {
            return regex.IsMatch(url);
        }
        public virtual string Convert(string url, string qs)
        {
            if (realPath.IndexOf("?") >= 0)
            {
                if (qs != null && qs.StartsWith("?"))
                {
                    qs = qs.Replace("?", "&");
                }
            }
            return string.Format("{0}{1}{2}", Globals.ApplicationPath + "/" + DynamicPath.GetPath(), regex.Replace(url, realPath), qs);
        }

        #endregion
    }
}