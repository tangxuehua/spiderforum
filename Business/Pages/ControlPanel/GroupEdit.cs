using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class GroupEditPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedCheckBox enabledCheckBox;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdmin))
            {
                throw new Exception("您没有修改版块或版块组的权限！");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new TRequest<Group>(GetValue<int>(ForumParameterName.GroupId))));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.Entity as Group);
        }

        #region Ajax Methods

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            Group group = Engine.Get<Group>(new TRequest<Group>(GetValue<int>(ForumParameterName.GroupId)));

            CheckData(group);

            group.Subject.Value = subjectTextBox.Value.Trim();
            group.Enabled.Value = enabledCheckBox.Checked ? (int)EnableStatus.Enable : (int)EnableStatus.Disable;
            Engine.Update(group);
        }

        #endregion

        private void BindData(Group group)
        {
            if (group != null)
            {
                subjectTextBox.Value = group.Subject.Value;
                enabledCheckBox.Checked = group.Enabled.Value == (int)EnableStatus.Enable ? true : false;
            }
        }
        private void CheckData(Group group)
        {
            if (group == null)
            {
                throw new Exception("版块组已经被删除！");
            }
            TRequest<Group> request = new TRequest<Group>();
            request.Data.Subject.Value = subjectTextBox.Value.Trim();
            EntityList groups = Engine.GetAll(request);
            if (groups.Count > 0 && ((Group)groups[0]).EntityId.Value != group.EntityId.Value)
            {
                throw new Exception("新版块组名称和已有的板块组名称重复！");
            }
        }
    }
}