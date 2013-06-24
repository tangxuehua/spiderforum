using System.Collections.Generic;

namespace System.Web.Core
{
    public class RequestDependency
    {
        #region Private Members

        private object sourceObject;
        private object targetObject;
        private string sourcePropertyName;
        private string targetPropertyName;

        #endregion

        #region Constructors

        public RequestDependency(object sourceObject, object targetObject, string sourcePropertyName, string targetPropertyName)
        {
            this.sourceObject = sourceObject;
            this.targetObject = targetObject;
            this.sourcePropertyName = sourcePropertyName;
            this.targetPropertyName = targetPropertyName;
        }

        #endregion

        #region Public Properties

        public object SourceObject
        {
            get
            {
                return sourceObject;
            }
            set
            {
                sourceObject = value;
            }
        }
        public object TargetObject
        {
            get
            {
                return targetObject;
            }
            set
            {
                targetObject = value;
            }
        }
        public string SourcePropertyName
        {
            get
            {
                return sourcePropertyName;
            }
            set
            {
                sourcePropertyName = value;
            }
        }
        public string TargetPropertyName
        {
            get
            {
                return targetPropertyName;
            }
            set
            {
                targetPropertyName = value;
            }
        }

        #endregion
    }
}