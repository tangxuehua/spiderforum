<%@ Page Language="C#" AutoEventWireup="true" Inherits="Forum.Business.CreateThreadPage, Forum.Business" MasterPageFile="~/Themes/Default/Masters/ContentMaster.master" %>
<%@Register tagPrefix="ctrl" tagName="Editor" src="~/themes/default/controls/Skin-HtmlEditor.ascx" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <ctrl:HeadControl runat="server" TitleResourceName="PageTitle_NewThread" />

    <table width="100%" class="formtbl" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td width="100px" align="right" valign="top"><ctrl:ResourceLabel CssClass="label" runat="Server" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_Thread_Subject" /></td>
            <td align="left"><ctrl:ValuedTextBox Runat="server" CssClass="txt Pecent80Width" id="subjectTextBox" MaxLength="60" /></td>
        </tr>
        <tr>
            <td width="100px" align="right" valign="top"><ctrl:ResourceLabel CssClass="label" runat="Server" ResourceFile="ControlPanelResources.xml" ResourceName="System_ControlPanel_Thread_Body" /></td>
            <td align="left"><ctrl:Editor Runat="server" id="bodyEditor" /></td>
        </tr>
        <tr>
            <td width="100px" align="right" valign="top"></td>
            <td align="left"><ctrl:Resourcebutton ID="SaveThreadButton" OnClientClick="return SubmitForm(this)" ResourceName="SaveThread" CssClass="NormalButton" Runat="server" /></td>
        </tr>
    </table>

    <script type="text/javascript">

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
                source.value = "发表帖子";
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
            var saveThreadButton = document.getElementById('<%= SaveThreadButton.ClientID %>');
            saveThreadButton.disabled = false;
            saveThreadButton.value = "发表帖子";
            if(result.error != null && result.error != "")
            {
                alert(result.error);
                return;
            }
            tinyMCE.activeEditor.isNotDirty = 1;
            RedirectToManageList(result.value);
        }
        function RedirectToManageList(threadListUrl)
        {
            self.location = threadListUrl;
        }
    </script>

</asp:Content>