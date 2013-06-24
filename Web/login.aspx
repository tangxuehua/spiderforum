<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="Forum.Business.LoginPage, Forum.Business" %>
<%@Register tagPrefix="ctrl" tagName="Footer" src="~/themes/default/controls/Skin-Footer.ascx" %>
<%@ Import Namespace="Forum.Business" %>
<%@ Import Namespace="System.Web.Core" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%= Head.Instance.Title %>
    </title>
    <link rel="shortcut icon" href="<%= Globals.ApplicationPath + "/favicon.ico" %>">
    <link rel="icon" type="image/gif" href="<%= Globals.ApplicationPath + "/animated_favicon1.gif" %>">
    <link type="text/css" rel="Stylesheet" href="<%= Globals.ApplicationPath + "/StaticFiles.ashx?s=DefaultThemeLoginCssFiles&amp;t=text/css&amp;v=001" %>" />
    <script type="text/javascript" src="<%= Globals.ApplicationPath + "/StaticFiles.ashx?s=GlobalJavaScriptFiles&amp;t=text/javascript&amp;v=001" %>"></script>
</head>
<body>
    <form id="aspnetForm" runat="server">
        <div class="wrap">
            <ctrl:HeadControl runat="server" TitleResourceName="PageTitle_Login" />

            <script type="text/javascript">
                <!--
                function CheckValue()
                {  
                    var userName = document.getElementById("<%= userName.ClientID%>");
                    if(userName.value.Trim().length==0)
                    {
                        alert("请填写用户名！");
                        userName.focus();
                        return false;
                    }
                    var password = document.getElementById("<%= password.ClientID%>");
                    if(password.value.Trim().length < 6 || password.value.Trim().length > 12)
                    {
                        alert("密码必须是在6到12位之间！");
                        password.focus();
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
                    
                    <%= ClientID %>.Submit('<%=ClientID %>', SubmitFormCallBack);
                    
                    return false;
                }
                function SubmitFormCallBack(result)
                {
                    if(result.error != null && result.error != "")
                    {
                        alert(result.error);
                        return;
                    }

                    RedirectToManageList(result.value);
                }
                function RedirectToManageList(redirectUrl)
                {
                    self.location = redirectUrl;
                }    
            //-->
            </script>

            <div class="conBox">
                <div class="leftBox">
                    <h1>
                        欢迎登录蜘蛛侠论坛！</h1>
                    <ul class="list">
                        <br>
                        <li><strong>论坛特色</strong>：界面整洁、功能实用</li>
                        <li><strong>论坛宗旨</strong>：为大家提供一个简洁大方的领域驱动设计（DDD）交流平台</li>
                    </ul>
                </div>
                <div class="rightBox">
                    <div class="righttitle">
                        <h2>通行证</h2>
                    </div>
                    <div class="login">
                        <p>
                            <ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="Login_UserName" />
                            <ctrl:ValuedTextBox ID="userName" TabIndex="1" MaxLength="50" class="i" runat="server" />
                        </p>
                        <p>
                            <ctrl:ResourceLiteral ID="ResourceLiteral2" runat="server" ResourceName="Login_Password" />
                            <ctrl:ValuedTextBox ID="password" TextMode="Password" TabIndex="2" MaxLength="50" class="i" runat="server" />
                        </p>
                        <p class="help"><ctrl:ValuedCheckBox ID="rememberMe" TabIndex="3" runat="server" />记住登录状态</p>
                        <p class="input"><ctrl:ResourceButton ID="loginButton" TabIndex="4" OnClientClick="return SubmitForm()" runat="server" /></p>
                    </div>
                    <div class="getReg">
                        <h1>还没有登录帐号？</h1><a href="<%= SiteUrls.Instance.Register %>">马上注册一个</a>
                    </div>
                </div>
            </div>

            <ctrl:Footer runat="server" />
        </div>
    </form>
</body>
</html>