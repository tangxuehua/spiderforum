<%@ Page Language="C#" Inherits="Forum.Business.ModulePermissionManagePage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<div class="AdminArea">
    <fieldset>
        <legend>模块权限管理</legend>                
        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="RoleNameCol">角色</th>
                            <th class="PermissionCol">用户管理</th>
                            <th class="PermissionCol">角色管理</th>
                            <th class="PermissionCol">角色授权管理</th>
                            <th class="PermissionCol">版块管理</th>
                            <th class="PermissionCol">版主管理</th>
                            <th class="PermissionCol">错误日志管理</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="RoleNameCol"><asp:HiddenField runat="server" ID="roleIDHidden" /><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td class="PermissionCol"><asp:CheckBox runat="server" ID="UserAdmin" /></td>
                    <td class="PermissionCol"><asp:CheckBox runat="server" ID="RoleAdmin" /></td>
                    <td class="PermissionCol"><asp:CheckBox runat="server" ID="RolePermissionAdmin" /></td>
                    <td class="PermissionCol"><asp:CheckBox runat="server" ID="SectionAdmin" /></td>
                    <td class="PermissionCol"><asp:CheckBox runat="server" ID="SectionAdminsAdmin" /></td>
                    <td class="PermissionCol"><asp:CheckBox runat="server" ID="ExceptionLogAdmin" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </ctrl:NoneStateRepeater>
        <div class="FormRow SubmitButtonRow">
            <ctrl:Resourcebutton id="saveButton" ResourceName="Save" CssClass="Button" Runat="server"></ctrl:Resourcebutton>
        </div>
    </fieldset>
</div>

</asp:Content>