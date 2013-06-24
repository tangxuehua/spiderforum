using System;
using System.Collections.Generic;

namespace System.Web.Core
{
    public interface ICondition
    {
        List<ICondition> ChildConditions
        {
            get;
        }
        bool IsValid(Request request);
        string GetConditionExpression(Request request);
        string GetConditionExpressionWithoutAliasName(Request request);
        bool HasInOperatorCondition
        {
            get;
        }
    }
}
