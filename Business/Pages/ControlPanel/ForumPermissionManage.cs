using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class ForumPermissionManagePage : ForumBasePage
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

            CheckBox currentCheckBox = item.FindControl("View") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.View);

            currentCheckBox = item.FindControl("CreateThread") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.CreateThread);

            currentCheckBox = item.FindControl("EditThread") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.EditThread);

            currentCheckBox = item.FindControl("StickThread") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.StickThread);

            currentCheckBox = item.FindControl("ModifyThreadStatus") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.ModifyThreadStatus);

            currentCheckBox = item.FindControl("DeleteThread") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.DeleteThread);

            currentCheckBox = item.FindControl("CloseThread") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.CloseThread);

            currentCheckBox = item.FindControl("EditPost") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.EditPost);

            currentCheckBox = item.FindControl("DeletePost") as CheckBox;
            currentCheckBox.Checked = permission.GetBit((long)PermissionType.DeletePost);
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
                            case "View":
                                permission.SetBit((long)PermissionType.View, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "CreateThread":
                                permission.SetBit((long)PermissionType.CreateThread, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "EditThread":
                                permission.SetBit((long)PermissionType.EditThread, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "StickThread":
                                permission.SetBit((long)PermissionType.StickThread, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "ModifyThreadStatus":
                                permission.SetBit((long)PermissionType.ModifyThreadStatus, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "DeleteThread":
                                permission.SetBit((long)PermissionType.DeleteThread, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "CloseThread":
                                permission.SetBit((long)PermissionType.CloseThread, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "EditPost":
                                permission.SetBit((long)PermissionType.EditPost, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                            case "DeletePost":
                                permission.SetBit((long)PermissionType.DeletePost, (c as CheckBox).Checked ? AccessControlEntry.Allow : AccessControlEntry.Deny);
                                break;
                        }
                    }

                    Engine.Update(permission);
                }
            }
        }
    }
}