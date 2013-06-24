using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class ModulePermissionManagePage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected ResourceButton saveButton;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            list.EnableViewState = true;
            list.ItemDataBound += new RepeaterItemEventHandler(list_ItemDataBound);
            saveButton.Click += new EventHandler(saveButton_Click);
        }
        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.RolePermissionAdmin))
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

        private void BindData(EntityList roles)
        {
            list.DataSource = RemoveNotAllowEditPermissionRole(roles);
            list.DataBind();
        }
        private EntityList RemoveNotAllowEditPermissionRole(EntityList allRoles)
        {
            EntityList filterRoles = new EntityList();
            foreach (Role role in allRoles)
            {
                if (role.IsRoleType((long)RoleType.AllowEditPermission))
                {
                    filterRoles.Add(role);
                }
            }
            return filterRoles;
        }
        private void list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            HiddenField roleIDHidden = item.FindControl("roleIDHidden") as HiddenField;
            roleIDHidden.Value = (item.DataItem as Role).EntityId.Value.ToString();

            Permission permission = RolePermissionManager.GetRolePermission((item.DataItem as Role).EntityId.Value);
            if (permission == null)
            {
                return;
            }

            CheckBox currentCheckBox = item.FindControl("UserAdmin") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.UserAdmin);

            currentCheckBox = item.FindControl("RoleAdmin") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.RoleAdmin);

            currentCheckBox = item.FindControl("RolePermissionAdmin") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.RolePermissionAdmin);

            currentCheckBox = item.FindControl("SectionAdmin") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.SectionAdmin);

            currentCheckBox = item.FindControl("SectionAdminsAdmin") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.SectionAdminsAdmin);

            currentCheckBox = item.FindControl("ExceptionLogAdmin") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.ExceptionLogAdmin);

        }
        public void saveButton_Click(object sender, EventArgs e)
        {
            Permission permission = null;
            HiddenField roleIDHidden = null;

            foreach (RepeaterItem item in list.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    roleIDHidden = item.FindControl("roleIDHidden") as HiddenField;
                    permission = RolePermissionManager.GetRolePermission(int.Parse(roleIDHidden.Value));
                    if (permission == null)
                    {
                        permission = new Permission();
                        permission.RoleId.Value = int.Parse(roleIDHidden.Value);
                        permission.AllowMask.Value = 0;
                        permission.DenyMask.Value = 0;
                        Engine.Create(permission);
                    }
                    foreach (Control c in item.Controls)
                    {
                        if (string.IsNullOrEmpty(c.ID))
                        {
                            continue;
                        }
                        switch (c.ID)
                        {
                            case "UserAdmin":
                                permission.SetBit((long)PermissionType.UserAdmin, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "RoleAdmin":
                                permission.SetBit((long)PermissionType.RoleAdmin, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "RolePermissionAdmin":
                                permission.SetBit((long)PermissionType.RolePermissionAdmin, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "SectionAdmin":
                                permission.SetBit((long)PermissionType.SectionAdmin, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "SectionAdminsAdmin":
                                permission.SetBit((long)PermissionType.SectionAdminsAdmin, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "ExceptionLogAdmin":
                                permission.SetBit((long)PermissionType.ExceptionLogAdmin, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                        }
                    }

                    Engine.Update(permission);
                }
            }
        }
    }
}