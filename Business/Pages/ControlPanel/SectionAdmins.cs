using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionAdminsPage : ForumBasePage
    {
        protected CheckBoxList userList;
        protected ResourceButton saveButton;

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            TRequest<SectionRoleUser> request = new TRequest<SectionRoleUser>();
            request.Data.SectionId.Value = GetValue<int>(ForumParameterName.SectionId);
            request.Data.RoleId.Value = GetValue<int>(ParameterName.RoleId);
            requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.EntityList);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            saveButton.Click += new EventHandler(SaveSectionAdmins);
        }
        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdminsAdmin))
            {
                throw new Exception("Access denied.");
            }
        }

        private void BindData(EntityList originalSectionRoleUsers)
        {
            int roleId = GetValue<int>(ParameterName.RoleId);
            userList.DataSource = UserManager.GetRoleUsers(roleId);
            userList.DataTextField = "NickName";
            userList.DataValueField = "EntityId";
            userList.DataBind();

            foreach (SectionRoleUser sectionRoleUser in originalSectionRoleUsers)
            {
                foreach (ListItem item in userList.Items)
                {
                    if (item.Value == sectionRoleUser.UserId.Value.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
        private void SaveSectionAdmins(object sender, EventArgs e)
        {
            //处理删除的版主
            bool exist = false;
            TRequest<SectionRoleUser> sruRequest = new TRequest<SectionRoleUser>();
            sruRequest.Data.SectionId.Value = GetValue<int>(ForumParameterName.SectionId);
            sruRequest.Data.RoleId.Value = GetValue<int>(ParameterName.RoleId);
            EntityList originalSectionRoleUsers = Engine.GetAll(sruRequest);
            foreach (SectionRoleUser sru in originalSectionRoleUsers)
            {
                exist = false;
                foreach (ListItem item in userList.Items)
                {
                    if (item.Selected && int.Parse(item.Value) == sru.UserId.Value)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    TRequest<SectionRoleUser> request = new TRequest<SectionRoleUser>();
                    request.Data.SectionId.Value = sruRequest.Data.SectionId.Value;
                    request.Data.RoleId.Value = sruRequest.Data.RoleId.Value;
                    request.Data.UserId.Value = sru.UserId.Value;
                    Engine.DeleteList(request);
                }
            }

            //处理新增的版主
            SectionRoleUser sectionRoleUser = null;
            int userId = 0;
            foreach (ListItem item in userList.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }
                userId = int.Parse(item.Value);
                exist = false;
                foreach (SectionRoleUser sru in originalSectionRoleUsers)
                {
                    if (userId == sru.UserId.Value)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    sectionRoleUser = new SectionRoleUser();
                    sectionRoleUser.SectionId.Value = sruRequest.Data.SectionId.Value;
                    sectionRoleUser.RoleId.Value = sruRequest.Data.RoleId.Value;
                    sectionRoleUser.UserId.Value = userId;
                    Engine.Create(sectionRoleUser);
                }
            }
        }
    }
}