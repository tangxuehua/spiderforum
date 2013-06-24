<%@ Page Language="C#" Inherits="Forum.Business.SectionAdminsPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<div class="AdminArea">
    <fieldset>
        <legend><ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="System_ControlPanel_SectionAdmins_ManageTitle" ResourceFile="ControlPanelResources.xml" /></legend>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel1" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_SectionAdmins_UserList" />
            <asp:CheckBoxList CssClass="CheckboxList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="6" ID="userList" />
        </div>
        <div class="FormRow SubmitButtonRow">
            <ctrl:Resourcebutton id="saveButton" ResourceName="Save" CssClass="Button" Runat="server"></ctrl:Resourcebutton>
        </div>
    </fieldset>
</div>

</asp:Content>