<%@ Page Language="C#" Inherits="Forum.Business.PostEditPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@Register tagPrefix="ctrl" tagName="Editor" src="~/controlpanelthemes/default/controls/Skin-HtmlEditor.ascx" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<div class="AdminArea">
    <fieldset>
        <legend><ctrl:ResourceLiteral runat="server" ResourceName="System_ControlPanel_Post_EditControlTitle" ResourceFile="ControlPanelResources.xml" /></legend>
        <div class="FormRow">
            <ctrl:ResourceLabel CssClass="FieldName" runat="Server" ControlToLabel="bodyEditor" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_Post_Body" />
            <br /><br /><ctrl:Editor Runat="server" id="bodyEditor" />
        </div>
        <div class="FormRow SubmitButtonRow">
            <ctrl:Resourcebutton ID="saveButton" OnClientClick="return SubmitForm(this)" ResourceName="Save" CssClass="Button" Runat="server"></ctrl:Resourcebutton>
        </div>
    </fieldset>
</div>

<script type="text/javascript">
<!--
    function SubmitForm(source)
    {
        source.disabled = true;
        source.value = "提交中...";
        var content = tinyMCE.activeEditor.getContent();
        var editor = document.getElementById("<%= bodyEditor.ClientID %>" + "_editor");
        editor.value = content;
        <%= ClientID %>.Save('<%=ClientID %>', SubmitFormCallBack);
        return false;
    }
    function SubmitFormCallBack(res)
    {
        var saveButton = document.getElementById('<%= saveButton.ClientID %>');
        saveButton.disabled = false;
        saveButton.value = "确定";
        if(res.error != null && res.error != "")
        {
            alert(res.error);
            return;
        }
        alert("回复修改成功！");
        tinyMCE.activeEditor.isNotDirty = 1;
        window.close();
    }
//-->
</script>

</asp:Content>