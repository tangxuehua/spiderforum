<%@ Page Language="C#" Inherits="Forum.Business.ThreadEditPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@Register tagPrefix="ctrl" tagName="Editor" src="~/controlpanelthemes/default/controls/Skin-HtmlEditor.ascx" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<div class="AdminArea">
    <fieldset>
        <legend><ctrl:ResourceLiteral runat="server" ResourceName="System_ControlPanel_Thread_EditControlTitle" ResourceFile="ControlPanelResources.xml" /></legend>
        <div class="FormRow">
            <ctrl:ResourceLabel CssClass="FieldName" runat="Server" ControlToLabel="subjectTextBox" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_Thread_Subject" />
            <ctrl:ValuedTextBox Runat="server" CssClass="InputField Pecent80Width" id="subjectTextBox" />
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel CssClass="FieldName" runat="Server" ControlToLabel="bodyEditor" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_Thread_Body" />
            <br /><br /><ctrl:Editor Runat="server" id="bodyEditor" />
        </div>
        <div class="FormRow SubmitButtonRow">
            <ctrl:Resourcebutton ID="saveButton" OnClientClick="return SubmitForm(this)" ResourceName="Save" CssClass="Button" Runat="server"></ctrl:Resourcebutton>
        </div>
    </fieldset>
</div>

<script type="text/javascript">
<!--
    function CheckValue()
    {
         var subjectTextBox = document.getElementById('<%= subjectTextBox.ClientID %>');
         var title = subjectTextBox.value;
         title = title.toText().Trim();
         if(title == null || title == "")
         {
               alert('标题不能为空!');
               subjectTextBox.focus();
               return false;
         }
         return true;
    }
    function SubmitForm(source)
    {
        source.disabled = true;
        source.value = "提交中...";
        if (!CheckValue())
        {
            source.disabled = false;
            source.value = "确定";
            return false;
        }

        var content = tinyMCE.activeEditor.getContent();
        var editor = document.getElementById("<%= bodyEditor.ClientID %>" + "_editor");
        editor.value = content;

        <%= ClientID %>.Save('<%=ClientID %>', SubmitFormCallBack);
        return false;
    }
    function SubmitFormCallBack(result)
    {
        var saveThreadButton = document.getElementById('<%= saveButton.ClientID %>');
        saveThreadButton.disabled = false;
        saveThreadButton.value = "确定";
        if(result.error != null && result.error != "")
        {
            alert(result.error);
            return;
        }
        tinyMCE.activeEditor.isNotDirty = 1;
        RedirectToManageList(result.value);
    }
    function RedirectToManageList(manageListUrl)
    {
        self.location = manageListUrl;
    }
    
//-->
</script>

</asp:Content>
