using System.Collections.Generic;

namespace System.Web.Core
{
    public interface IPropertyCondition : IChildCondition
    {
        Type DataValidatorType
        {
            get;
            set;
        }
        string PropertyPath
        {
            get;
            set;
        }
        object PropertyValue
        {
            get;
            set;
        }
    }
}
