using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionEditPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedTextBox groupTextBox;
        protected ValuedCheckBox enabledCheckBox;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdmin))
            {
                throw new Exception("您没有权限管理论坛版块或版块组！");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new TRequest<Section>(GetValue<int>(ForumParameterName.SectionId))));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.Entity as Section);
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            Section section = Engine.Get<Section>(new TRequest<Section>(GetValue<int>(ForumParameterName.SectionId)));

            CheckData(section);

            section.Enabled.Value = enabledCheckBox.Checked ? 1 : 0;
            section.Subject.Value = subjectTextBox.Value;
            Engine.Update(section);
        }

        private void BindData(Section section)
        {
            if (section != null)
            {
                subjectTextBox.Value = section.Subject.Value;
                enabledCheckBox.Checked = section.Enabled.Value == 1 ? true : false;
                Group group = Engine.Get<Group>(section.GroupId.Value);
                if (group != null)
                {
                    groupTextBox.Value = group.Subject.Value;
                }
            }
        }
        private void CheckData(Section section)
        {
            if (section == null)
            {
                throw new Exception("版块组已经被删除！");
            }
            TRequest<Section> request = new TRequest<Section>();
            request.Data.Subject.Value = subjectTextBox.Value.Trim();
            EntityList sections = Engine.GetAll(request);
            if (sections.Count > 0 && ((Section)sections[0]).EntityId.Value != section.EntityId.Value)
            {
                throw new Exception("新版块名称和已有的版块名称重复！");
            }
        }
    }
}