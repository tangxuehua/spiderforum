<%@ Page Language="C#" Inherits="Forum.Business.EditProfilePage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/controlpanelthemes/default/masters/ControlPanelIFrameContentMaster.master" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <script language="javascript" type="text/javascript">
    <!--
        function SubmitForm()
        {
            var nickNameValuedTextBox = document.getElementById('<%= nickNameValuedTextBox.ClientID %>');
            if(nickNameValuedTextBox.value.Trim().length == 0)
            {
                alert('用户昵称不能为空！');
                nickNameValuedTextBox.focus();
                return false;
            }
            <%= ClientID %>.SaveProfile('<%=ClientID %>', nickNameValuedTextBox.value.Trim(), SubmitFormCallBack);
            return false;
        }
        function SubmitFormCallBack(res)
        {
            if(res.error != null && res.error != "")
            {
                alert(res.error);
                return;
            }
            alert('修改成功！');
        }
    //-->
    </script>

    <div id="EditForumProfile" class="AdminArea">
        <fieldset>
            <legend><ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="EditProfile_Title" ResourceFile="ControlPanelResources.xml" /></legend> 
            <div class="FormRow">
                <div class="LargerFieldNameRequireCharacter">
                    <ctrl:ResourceLabel ID="ResourceLabel1" runat="Server" CssClass="RedColor" ResourceFile="ControlPanelResources.xml" ResourceName="RequireCharacter" />
                    <ctrl:ResourceLabel ID="ResourceLabel2" runat="Server" ResourceFile="ControlPanelResources.xml" ResourceName="EditProfile_NickName" />
                </div>
                <ctrl:ValuedTextBox Runat="server" MaxLength="12" CssClass="InputField SmallWidth" id="nickNameValuedTextBox" />
            </div>
	        <div class="FormRow SubmitButtonRow">
                <ctrl:Resourcebutton id="SaveButton" ResourceName="Save" OnClientClick="return SubmitForm()" ResourceFile="ControlPanelResources.xml" CssClass="Button" Runat="server" />
            </div>
        </fieldset>
    </div>

</asp:Content>
