using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class RoleEditPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new TRequest<Role>(GetValue<int>(ParameterName.RoleId))));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.Entity as Role);
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            Role role = Engine.Get<Role>(UrlManager.Instance.GetParameterValue<int>(ParameterName.RoleId));

            CheckData(role);

            role.Name.Value = subjectTextBox.Value;
            Engine.Update(role);
        }

        private void BindData(Role role)
        {
            if (role != null)
            {
                subjectTextBox.Value = role.Name.Value;
            }
        }
        private void CheckData(Role role)
        {
            if (role == null)
            {
                throw new Exception("��Ҫ�޸ĵĽ�ɫ�Ѿ���ɾ����");
            }
            TRequest<Role> request = new TRequest<Role>();
            request.Data.Name.Value = subjectTextBox.Value.Trim();
            TEntityList<Role> roles = Engine.GetAll<Role>(request);
            if (roles.Count > 0 && roles[0].EntityId.Value != role.EntityId.Value)
            {
                throw new Exception("�½�ɫ���ƺ����еĽ�ɫ�����ظ���");
            }
        }
    }
}