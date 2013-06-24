using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.Core;
using System.Web;

namespace Forum.Business
{
    public class RegisterPage : ForumBasePage
    {
        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Submit()
        {
            HttpRequest request = HttpContext.Current.Request;

            //Validate the AuthCode.
            if (CookieManager.GetCookieValue("AuthCode") != request["number"])
            {
                throw new Exception("验证码输入不正确！");
            }

            if (MemberManager.GetMember(request["User_Account"]) != null)
            {
                throw new Exception("该用户名已经被人注册！");
            }

            //Get Member information.
            Member newMember = new Member();
            newMember.MemberId.Value = Guid.NewGuid();
            newMember.MemberName.Value = request["User_Account"];
            newMember.Password.Value = request["User_Password"];

            //Get MemberInfo information.
            MemberInfo memberInfo = new MemberInfo();
            Hashtable memberAttributes = new Hashtable();
            memberAttributes["NickName"] = request["User_NickName"];
            memberInfo.MemberId = newMember.MemberId.Value;
            memberInfo.MemberAttributes = memberAttributes;

            //Create member in database.
            MemberManager.Create(newMember, memberInfo);

            //Login current user.
            MemberManager.Logout();
            MemberManager.Login(newMember.MemberName.Value);
        }
    }
}