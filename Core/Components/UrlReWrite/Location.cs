using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.Core
{
    public class Location : IEnumerable
    {
        #region Private Members

        private Regex regex;
        private string path;
        private bool exclude = false;
        private ArrayList urls = new ArrayList();

        #endregion

        #region Constructors

        public Location(string path, bool exclude)
        {
            this.path = path;
            this.exclude = exclude;
            if(!string.IsNullOrEmpty(path))
            {
                regex = new Regex("^" + path, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get { return urls.Count; }
        }
        public bool Exclude
        {
            get { return exclude;}
        }
        public string Path
        {
            get { return path;}
        }

        #endregion

        #region Public Methods

        public void Add(ReWrittenUrl url)
        {
            urls.Add(url);
        }
        public bool IsMatch(string url)
        {
            return regex.IsMatch(url);
        }
        public IEnumerator GetEnumerator()
        {
            return urls.GetEnumerator();
        }
        public virtual string ReWriteUrl(string path, string queryString)
        {
            string newPath = null;
            foreach (ReWrittenUrl url in this)
            {
                if (url.IsMatch(path))
                {
                    newPath = url.Convert(path, queryString);
                    break;
                }
            }
            return newPath;
        }

        #endregion
    }
}