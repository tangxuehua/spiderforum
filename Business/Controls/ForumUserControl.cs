using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;
using System.Web;

namespace Forum.Business
{
    public class ForumUserControl : BaseUserControl
    {
        protected ForumUser CurrentUser
        {
            get
            {
                return HttpContext.Current.User as ForumUser;
            }
        }

        protected bool ValidatePermission(PermissionType permission)
        {
            return CurrentUser.GetPermissions().ValidatePermission((long)permission);
        }
        protected bool ValidatePermission(PermissionType permission, Entity entity)
        {
            return CurrentUser.GetPermissions(entity).ValidatePermission((long)permission);
        }
    }
}