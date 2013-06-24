using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class UserRolesPage : ForumBasePage
    {
        protected CheckBoxList roleList;
        protected ResourceButton saveButton;

        private int userId = UrlManager.Instance.GetParameterValue<int>(ParameterName.UserId);

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            saveButton.Click += new EventHandler(SaveUserRoles);
        }
        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.UserAdmin))
            {
                throw new Exception("Access denied.");
            }
            BindData();
        }

        private void BindData()
        {
            roleList.DataSource = RemoveCoreRoles(Engine.GetAll<Role>(new TRequest<Role>()));
            roleList.DataTextField = "Name";
            roleList.DataValueField = "EntityId";
            roleList.DataBind();

            foreach (UserAndRole userAndRole in RoleManager.GetUserRoles(userId))
            {
                foreach (ListItem item in roleList.Items)
                {
                    if (item.Value == userAndRole.Role.EntityId.Value.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
        private void SaveUserRoles(object sender, EventArgs e)
        {
            EntityList originalUserRoles = RemoveCoreRoles(RoleManager.GetUserRoles(userId));

            //处理删除的角色
            bool exist = false;
            foreach (UserAndRole userAndRole in originalUserRoles)
            {
                exist = false;
                foreach (ListItem item in roleList.Items)
                {
                    if (item.Selected && int.Parse(item.Value) == userAndRole.Role.EntityId.Value)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    RoleManager.RemoveUserFromRole(userId, userAndRole.Role.EntityId.Value);
                }
            }

            //处理新增的角色
            int roleId = 0;
            UserRole userRole = null;
            foreach (ListItem item in roleList.Items)
            {
                if (!item.Selected)
                {
                    continue;
                }
                roleId = int.Parse(item.Value);
                exist = false;
                foreach (UserAndRole userAndRole in originalUserRoles)
                {
                    if (roleId == userAndRole.Role.EntityId.Value)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    userRole = new UserRole();
                    userRole.UserId.Value = userId;
                    userRole.RoleId.Value = roleId;
                    Engine.Create(userRole);
                }
            }
        }
        private EntityList RemoveCoreRoles(TEntityList<Role> roles)
        {
            EntityList filterRoles = new EntityList();
            foreach (Role role in roles)
            {
                if (role.IsRoleType((long)RoleType.AllowVisible))
                {
                    filterRoles.Add(role);
                }
            }
            return filterRoles;
        }
        private TEntityList<UserAndRole> RemoveCoreRoles(TEntityList<UserAndRole> roles)
        {
            TEntityList<UserAndRole> filterRoles = new TEntityList<UserAndRole>();
            foreach (UserAndRole userAndRole in roles)
            {
                if (userAndRole.Role.IsRoleType((long)RoleType.AllowVisible))
                {
                    filterRoles.Add(userAndRole);
                }
            }
            return filterRoles;
        }
    }
}