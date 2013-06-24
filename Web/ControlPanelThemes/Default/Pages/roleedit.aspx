<%@ Page Language="C#" Inherits="Forum.Business.RoleEditPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script type="text/javascript">
<!--
    function CheckValue()
    {  
        if(document.getElementById('<%= subjectTextBox.ClientID %>').value == "")
        {
            alert("角色名称不能为空！");
            return false;
        }

        return true;
    }
    function SubmitForm()
    {
        if (!CheckValue())
        {
            return false;
        }
        
        <%= ClientID %>.Save('<%=ClientID %>', SubmitFormCallBack);
        
        return false;
    }
    function SubmitFormCallBack(res)
    {
        if(res.error != null && res.error != "")
        {
            alert(res.error);
            return false;
        }

        RedirectToManageList();
        return false;
    }
    function RedirectToManageList()
    {
        self.location = GetManageList();
        return false;
    }
    function GetManageList()
    {
        return '<%= UrlManager.Instance.FormatUrl("role_list") %>';
    }
    
//-->
</script>

<div class="AdminArea">
    <fieldset>
        <legend><ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="System_ControlPanel_Role_EditControlTitle" ResourceFile="ControlPanelResources.xml" /></legend>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel1" CssClass="FieldName" runat="Server" ControlToLabel="subjectTextBox" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_Role_Name" />
            <ctrl:ValuedTextBox Runat="server" CssClass="InputField MiddleWidth" id="subjectTextBox" />
        </div>
        <div class="FormRow SubmitButtonRow">
            <ctrl:Resourcebutton id="saveButton" OnClientClick="return SubmitForm()" ResourceName="Save" CssClass="Button" Runat="server"></ctrl:Resourcebutton>
        </div>
    </fieldset>
</div>

</asp:Content>