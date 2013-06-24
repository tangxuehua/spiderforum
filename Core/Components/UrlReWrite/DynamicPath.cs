using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.Core
{
    public class DynamicPath
    {
        #region Private Members

        private string pathFormat;
        private IObjectsProvider valueProvider;

        #endregion

        #region Constructors

        public DynamicPath(string pathFormat, IObjectsProvider valueProvider)
        {
            this.pathFormat = pathFormat;
            this.valueProvider = valueProvider;
        }

        #endregion

        #region Public Properties

        public string PathFormat
        {
            get { return pathFormat; }
            set { pathFormat = value; }
        }
        public IObjectsProvider ValueProvider
        {
            get { return valueProvider; }
            set { valueProvider = value; }
        }

        #endregion

        #region Public Methods

        public string GetPath()
        {
            if (!string.IsNullOrEmpty(pathFormat) && valueProvider != null)
            {
                return string.Format(pathFormat, valueProvider.GetObjects());
            }
            return null;
        }

        #endregion
    }
}