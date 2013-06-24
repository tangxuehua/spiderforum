using System;
using System.Collections.Generic;

namespace System.Web.Core
{
    public interface IFieldCondition : ICondition
    {
        string PropertyPath
        {
            get;
            set;
        }
        string ParameterName
        {
            get;
        }
        string DbOperator
        {
            get;
            set;
        }
        object PropertyValue
        {
            get;
            set;
        }
        object GetConditionValue(Request request);
    }
}
