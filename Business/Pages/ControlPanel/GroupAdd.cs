using System;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class GroupAddPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedCheckBox enabledCheckBox;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdmin))
            {
                throw new Exception("��û���޸İ��������Ȩ�ޣ�");
            }
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            CheckData();

            Group group = new Group();
            group.Subject.Value = subjectTextBox.Value.Trim();
            group.Enabled.Value = enabledCheckBox.Checked ? (int)EnableStatus.Enable : (int)EnableStatus.Disable;
            Engine.Create(group);
        }

        private void CheckData()
        {
            TRequest<Group> request = new TRequest<Group>();
            request.Data.Subject.Value = subjectTextBox.Value.Trim();
            if (Engine.GetAll(request).Count > 0)
            {
                throw new Exception("��Ҫ��ӵİ�����Ѿ����ڣ�");
            }
        }
    }
}