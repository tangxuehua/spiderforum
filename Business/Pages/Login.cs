using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class LoginPage : ForumBasePage
    {
        protected ValuedTextBox userName;
        protected ValuedTextBox password;
        protected ValuedCheckBox rememberMe;

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public string Submit()
        {
            if (!MemberManager.ValidateMember(userName.Value, password.Value))
            {
                throw new Exception("�û�����������Ч��");
            }

            Member member = MemberManager.GetMember(userName.Value);
            ForumUser user = UserManager.GetUser(member.MemberId.Value) as ForumUser;
            if (user.UserStatus.Value == (int)UserStatus.Locked)
            {
                throw new Exception("�����ʺ��Ѿ������ᣬ����ϵϵͳ����Ա��");
            }

            MemberManager.Logout();
            if (rememberMe.Value == "True")
            {
                MemberManager.Login(userName.Value, 365);
            }
            else
            {
                MemberManager.Login(userName.Value);
            }

            return FormsAuthentication.GetRedirectUrl(member.MemberId.Value.ToString(), false);
        }
    }
}