<%@ Control Language="C#" Inherits="Forum.Business.ForumUserControl, Forum.Business" AutoEventWireup="true" %>

<ctrl:ResourceLink runat="server" UrlName="home" ResourceName="Home" />
»¶Ó­Äú:<%= CurrentUser.NickName.Value %>
<ctrl:ResourceLink runat="server" UrlName="logout" ResourceName="Logout" />
<ctrl:ResourceLink runat="server" UrlName="controlpanel" ResourceName="ControlPanel" />