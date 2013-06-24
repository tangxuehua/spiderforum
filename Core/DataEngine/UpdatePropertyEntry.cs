using System;

namespace System.Web.Core
{
    public class UpdatePropertyEntry
    {
        #region Private Members

        private string entityPropertyPath = string.Empty;
        private object propertyValue = null;

        #endregion

        #region Constructors

        public UpdatePropertyEntry()
        {
        }
        public UpdatePropertyEntry(string entityPropertyPath, object propertyValue)
        {
            this.entityPropertyPath = entityPropertyPath;
            this.propertyValue = propertyValue;
        }

        #endregion

        #region Public Properties

        public string EntityPropertyPath
        {
            get
            {
                return entityPropertyPath;
            }
            set
            {
                entityPropertyPath = value;
            }
        }
        public object PropertyValue
        {
            get
            {
                return propertyValue;
            }
            set
            {
                propertyValue = value;
            }
        }

        #endregion
    }
}