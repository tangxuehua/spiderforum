<%@ Page Language="C#" Inherits="Forum.Business.ForumPermissionManagePage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <div class="AdminArea">
        <fieldset>
            <legend>��̳����Ȩ�޹���</legend>                
            <ctrl:NoneStateRepeater id="list" runat="server">
                <HeaderTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                        <thead>
                            <tr>
                                <th class="RoleNameCol">��ɫ</th>
                                <th class="PermissionCol">���</th>
                                <th class="PermissionCol">����</th>
                                <th class="PermissionCol">�༭</th>
                                <th class="PermissionCol">�ö�</th>
                                <th class="PermissionCol">�޸�״̬</th>
                                <th class="PermissionCol">ɾ��</th>
                                <th class="PermissionCol">����</th>
                                <th class="PermissionCol">�༭�ظ�</th>
                                <th class="PermissionCol">ɾ���ظ�</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="RoleNameCol"><asp:HiddenField runat="server" ID="roleIDHidden" /><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="View" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="CreateThread" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="EditThread" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="StickThread" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="ModifyThreadStatus" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="DeleteThread" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="CloseThread" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="EditPost" /></td>
                        <td class="PermissionCol"><asp:CheckBox runat="server" ID="DeletePost" /></td>                    
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