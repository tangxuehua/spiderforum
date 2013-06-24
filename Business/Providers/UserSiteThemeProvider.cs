using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Core;

namespace Forum.Business
{
    public class UserSiteThemeProvider : IObjectsProvider
    {
        #region IObjectsProvider 成员

        public object[] GetObjects()
        {
            List<object> objects = new List<object>();
            ForumUser user = HttpContext.Current.User as ForumUser;
            if (user != null && !string.IsNullOrEmpty(user.SiteTheme.Value))
            {
                objects.Add(user.SiteTheme.Value);
            }
            else
            {
                objects.Add("default");
            }
            return objects.ToArray();
        }

        #endregion
    }
}
