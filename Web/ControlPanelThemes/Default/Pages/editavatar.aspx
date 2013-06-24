<%@ Page Language="C#" Inherits="Forum.Business.EditAvatarPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <script type="text/C#" runat="server">
        private void BindUserAvatar(object sender, EventArgs e)
        {
            UserAvatar userAvatar = sender as UserAvatar;
            if (userAvatar != null)
            {
                userAvatar.UserId = CurrentUser.EntityId.Value;
                userAvatar.AvatarFileName = CurrentUser.AvatarFileName.Value;
            }
        }
        
    </script>
        
    <div id="EditAvatar" class="AdminArea">
        <fieldset>
            <legend>
                <ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="EditAvatar_Title" ResourceFile="ControlPanelResources.xml" />
            </legend>
            <table width="100%" height="240px" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="30%" class="avatorRowLeft" align="center" valign="top">
                        <ctrl:UserAvatar ID="UserAvatar1" Width="180" Height="180" runat="server" OnPreRender="BindUserAvatar" />
                    </td>
                    <td width="70%" class="avatorRowRight" valign="top">
                        <asp:FileUpload runat="server" CssClass="AvatarFileUpload" ID="avatarUpload" />
                        <div class="EditAvatar_SaveButtonRow">
                            <ctrl:ResourceButton ID="deleteButton" ResourceName="DeleteAvatar" ResourceFile="ControlPanelResources.xml"
                                CssClass="Button" runat="server"></ctrl:ResourceButton>&nbsp;&nbsp;&nbsp;&nbsp;
                            <ctrl:ResourceButton ID="saveButton" ResourceName="SaveAvatar" ResourceFile="ControlPanelResources.xml"
                                CssClass="Button" runat="server"></ctrl:ResourceButton>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>