using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionAddPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedCheckBox enabledCheckBox;
        protected ValuedDropDownList groupDropDownList;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdmin))
            {
                throw new Exception("您没有权限管理论坛版块或版块组！");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, new TRequest<Group>()));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.EntityList);
        }

        private void BindData(EntityList groups)
        {
            groupDropDownList.DataSource = groups;
            groupDropDownList.DataTextField = "Subject";
            groupDropDownList.DataValueField = "EntityId";
            groupDropDownList.DataBind();

            int? groupId = UrlManager.Instance.GetParameterValue<int?>(ForumParameterName.GroupId.ToString().ToLower());
            if (groupId.HasValue)
            {
                groupDropDownList.Value = groupId.Value.ToString();
            }
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            CheckData();

            Section section = new Section();
            section.Subject.Value = subjectTextBox.Value;
            section.Enabled.Value = enabledCheckBox.Checked ? 1 : 0;
            section.GroupId.Value = int.Parse(groupDropDownList.Value);
            Engine.Create(section);
        }

        private void CheckData()
        {
            if (string.IsNullOrEmpty(groupDropDownList.Value))
            {
                throw new Exception("请选择一个版块组！");
            }

            TRequest<Section> request = new TRequest<Section>();
            request.Data.Subject.Value = subjectTextBox.Value.Trim();
            if (Engine.GetAll(request).Count > 0)
            {
                throw new Exception("您要添加的版块已经存在！");
            }
        }
    }
}