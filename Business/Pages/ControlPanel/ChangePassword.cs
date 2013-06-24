using System;
using System.Web;
using System.Web.Core;

namespace Forum.Business
{
    public class ChangePasswordPage : ForumBasePage
    {
        protected ValuedTextBox oldPasswordTextBox;
        protected ValuedTextBox newPasswordTextBox;
        protected ValuedTextBox newPasswordConfirmTextBox;

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            if (CurrentUser.IsAnonymous)
            {
                throw new Exception("您还没有登陆！");
            }

            Member member = MemberManager.GetMember(CookieManager.GetCookieValue("MemberName"));

            if (member == null)
            {
                throw new Exception("当前登陆用户不存在！");
            }

            if (!MemberManager.ValidateMember(member.MemberId.Value, oldPasswordTextBox.Value))
            {
                throw new Exception("原密码错误！");
            }

            MemberManager.ChangeMemberPassword(member, newPasswordTextBox.Value);
        }
    }
}