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
                throw new Exception("����û�е�½��");
            }

            Member member = MemberManager.GetMember(CookieManager.GetCookieValue("MemberName"));

            if (member == null)
            {
                throw new Exception("��ǰ��½�û������ڣ�");
            }

            if (!MemberManager.ValidateMember(member.MemberId.Value, oldPasswordTextBox.Value))
            {
                throw new Exception("ԭ�������");
            }

            MemberManager.ChangeMemberPassword(member, newPasswordTextBox.Value);
        }
    }
}