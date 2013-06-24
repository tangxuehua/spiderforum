using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Core;

namespace Forum.Business
{
    public class UserControlPanelThemeProvider : IObjectsProvider
    {
        #region IObjectsProvider 成员

        public object[] GetObjects()
        {
            List<object> objects = new List<object>();
            objects.Add("default");
            return objects.ToArray();
        }

        #endregion
    }
}
