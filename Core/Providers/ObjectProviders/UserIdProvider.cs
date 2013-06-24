using System;
using System.Collections.Generic;
using System.Web;

namespace System.Web.Core
{
    public class UserIdProvider : IObjectsProvider
    {
        #region IObjectsProvider 成员

        public object[] GetObjects()
        {
            List<object> objects = new List<object>();
            objects.Add(((User)HttpContext.Current.User).EntityId.Value);
            return objects.ToArray();
        }

        #endregion
    }
}
