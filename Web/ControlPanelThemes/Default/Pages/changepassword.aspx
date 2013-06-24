<%@ Page Language="C#" Inherits="Forum.Business.ChangePasswordPage, Forum.Business" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <script type="text/javascript">
        function CheckValue()
        {           
            var oldPassword = document.getElementById("<%=oldPasswordTextBox.ClientID %>").value;
            oldPassword = oldPassword.replace(/(^\s*)|(\s*$)/g,"");
            if(oldPassword.length<6 || oldPassword.length>12)
            {
                alert("����6-12λ");
                document.getElementById("<%=oldPasswordTextBox.ClientID %>").focus();
                return false;
            }

            var pwd1=document.getElementById("<%=newPasswordTextBox.ClientID %>").value;
            pwd1=pwd1.replace(/(^\s*)|(\s*$)/g,"");

            if(pwd1.length<6 || pwd1.length>12){
                alert("����6-12λ");
                document.getElementById("<%=newPasswordTextBox.ClientID %>").focus();
                return false;
            }

            var pwd2=document.getElementById("<%=newPasswordConfirmTextBox.ClientID %>").value;
            pwd2=pwd2.replace(/(^\s*)|(\s*$)/g,"");
            if(pwd1!=pwd2){
                alert("������������Ĳ�һ��,����������!");
                document.getElementById("<%=newPasswordConfirmTextBox.ClientID %>").focus();
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

            alert("�����޸ĳɹ���");
            return false;
        }
    </script>

    <div class="AdminArea">
        <fieldset>
            <legend>
                <ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="ChangePassword_Title" ResourceFile="ControlPanelResources.xml" />
            </legend>
            <div class="FormRow">
                <ctrl:ResourceLabel ID="ResourceLabel1" CssClass="LargerFieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                    ResourceName="ChangePassword_OldPassword" />
                <ctrl:ValuedTextBox runat="server" TextMode="Password" MaxLength="15" CssClass="InputField SmallWidth"
                    ID="oldPasswordTextBox" />
            </div>
            <div class="FormRow">
                <ctrl:ResourceLabel ID="ResourceLabel2" CssClass="LargerFieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                    ResourceName="ChangePassword_NewPassword" />
                <ctrl:ValuedTextBox runat="server" TextMode="Password" MaxLength="15" CssClass="InputField SmallWidth"
                    ID="newPasswordTextBox" />
            </div>
            <div class="FormRow">
                <ctrl:ResourceLabel ID="ResourceLabel3" CssClass="LargerFieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                    ResourceName="ChangePassword_NewPasswordConfirm" />
                <ctrl:ValuedTextBox runat="server" TextMode="Password" MaxLength="15" CssClass="InputField SmallWidth"
                    ID="newPasswordConfirmTextBox" />
            </div>
            <div class="FormRow SubmitButtonRow">
                <div class="FormRow MiddleWidth">
                    <ctrl:ResourceButton OnClientClick="return SubmitForm()" ID="saveButton" ResourceName="Save" CssClass="Button" runat="server"></ctrl:ResourceButton>
                </div>
            </div>
        </fieldset>
    </div>

</asp:Content>