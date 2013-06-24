using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class RoleListPage : ForumBasePage
    {
        protected NoneStateRepeater list;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.RoleAdmin))
            {
                throw new Exception("Access denied.");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, new TRequest<Role>()));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.EntityList);
        }

        #region Ajax Methods

        [AjaxMethod]
        public void DeleteRole(int roleId)
        {
            Role role = Engine.Get<Role>(roleId);
            if (role == null)
            {
                throw new Exception("您要删除的角色不存在！");
            }
            if (!role.IsRoleType((long)RoleType.AllowDelete))
            {
                throw new Exception("该角色不允许被删除！");
            }

            TRequest<SectionRoleUser> sectionRoleUserRequest = new TRequest<SectionRoleUser>();
            sectionRoleUserRequest.Data.RoleId.Value = roleId;

            TRequest<Permission> permissionRequest = new TRequest<Permission>();
            permissionRequest.Data.RoleId.Value = roleId;

            TRequest<UserRole> userRoleRequest = new TRequest<UserRole>();
            userRoleRequest.Data.RoleId.Value = roleId;

            Engine.Executes(BinderBuilder.BuildDeleteListBinder(sectionRoleUserRequest),
                                BinderBuilder.BuildDeleteListBinder(permissionRequest),
                                BinderBuilder.BuildDeleteListBinder(userRoleRequest),
                                BinderBuilder.BuildDeleteBinder<Role>(roleId));

        }
        [AjaxMethod]
        public void DeleteRoles(string items)
        {
            if (string.IsNullOrEmpty(items))
            {
                return;
            }
            int entityId = 0;
            foreach (string item in items.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
            {
                entityId = Globals.ChangeType<int>(item);
                if (entityId > 0)
                {
                    DeleteRole(entityId);
                }
            }
        }

        #endregion

        private void BindData(EntityList roles)
        {
            list.DataSource = RemoveCoreRoles(roles);
            list.DataBind();
        }
        private EntityList RemoveCoreRoles(EntityList allRoles)
        {
            EntityList filterRoles = new EntityList();
            foreach (Role role in allRoles)
            {
                if (role.IsRoleType((long)RoleType.AllowVisible))
                {
                    filterRoles.Add(role);
                }
            }
            return filterRoles;
        }
    }
}