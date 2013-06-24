using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class LogoutPage : ForumBasePage
    {
        protected override void OnFirstLoad()
        {
            MemberManager.Logout();
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}