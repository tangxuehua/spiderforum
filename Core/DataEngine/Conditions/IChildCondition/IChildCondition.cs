using System;
using System.Collections.Generic;

namespace System.Web.Core
{
    public interface IChildCondition
    {
        bool IsValid(Request request);
        List<IChildCondition> ChildConditions
        {
            get;
        }
    }
}
