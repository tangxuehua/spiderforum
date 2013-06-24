using System.Collections.Generic;

namespace System.Web.Core
{
    public class PropertyCondition : IPropertyCondition
    {
        #region Private Members

        private Type dataValidatorType = null;
        private string propertyPath = null;
        private object propertyValue = null;
        private List<IChildCondition> childConditions = new List<IChildCondition>();

        #endregion

        #region IPropertyCondition 成员

        public Type DataValidatorType
        {
            get
            {
                return dataValidatorType;
            }
            set
            {
                dataValidatorType = value;
            }
        }
        public string PropertyPath
        {
            get
            {
                return propertyPath;
            }
            set
            {
                propertyPath = value;
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

        #region IChildCondition 成员

        public bool IsValid(Request request)
        {
            if (DataValidatorType != null)
            {
                IDataValidator validator = Activator.CreateInstance(DataValidatorType) as IDataValidator;
                if (validator != null)
                {
                    if (!string.IsNullOrEmpty(PropertyPath) && PropertyValue != null)
                    {
                        object propertyValue = Configuration.Instance.GetPropertyPathValue(request, PropertyPath);
                        return validator.Validate(propertyValue) && propertyValue.ToString() == PropertyValue.ToString();
                    }
                    else if (!string.IsNullOrEmpty(PropertyPath))
                    {
                        return validator.Validate(Configuration.Instance.GetPropertyPathValue(request, PropertyPath));
                    }
                }
                return false;
            }
            return false;
        }
        public List<IChildCondition> ChildConditions
        {
            get
            {
                return childConditions;
            }
        }

        #endregion
    }
}
