<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="Forum.Business.RegisterPage, Forum.Business" %>
<%@Register tagPrefix="ctrl" tagName="Footer" src="~/themes/default/controls/Skin-Footer.ascx" %>
<%@ Import Namespace="System.Web.Core" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="zh-CN" lang="zh-CN">
<head runat="server">
    <title><%= Head.Instance.Title %></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="<%= Globals.ApplicationPath + "/favicon.ico" %>">
    <link rel="icon" type="image/gif" href="<%= Globals.ApplicationPath + "/animated_favicon1.gif" %>">
    <link type="text/css" rel="Stylesheet" href="<%= Globals.ApplicationPath + "/StaticFiles.ashx?s=DefaultThemeRegisterCssFiles&amp;t=text/css&amp;v=001" %>" />
    <script type="text/javascript" src="<%= Globals.ApplicationPath + "/StaticFiles.ashx?s=GlobalJavaScriptFiles&amp;t=text/javascript&amp;v=001" %>"></script>
</head>
<body>
    <form id="aspnetForm" runat="server">
        <ctrl:HeadControl runat="server" TitleResourceName="PageTitle_Register" />

        <script type="text/javascript">

            function changeAuthCode() {
                return changeAuthCodeByAspx("authCode.ashx");
            }
            function changeAuthCodeByAspx(aspxFile) {
                var num = new Date().getTime();
                var rand = Math.round(Math.random() * 10000);
                num = num + rand;
                document.getElementById("yzm").src = aspxFile + "?tag=" + num;
                document.getElementById("identifier").value = num;
                return false;
            }

            function setCurrentItem(row_id) {
                document.getElementById("row_user").style.backgroundColor = "";
                document.getElementById("row_nickname").style.backgroundColor = "";
                document.getElementById("row_pass").style.backgroundColor = "";
                document.getElementById("row_pass_confirm").style.backgroundColor = "";
                document.getElementById("row_yzm").style.backgroundColor = "";
                document.getElementById(row_id).style.backgroundColor = "#EDF2F7";
            }

            //check username
            function checkUserNameInternal(user_account) {
                var user_name = user_account.value.toLowerCase();
                var pattern=/^[a-zA-Z][a-zA-Z0-9_]{1,14}[a-zA-Z0-9]$/i;
                var patternLastChar = /^[a-zA-Z0-9_]{1,15}_$/i;
                var patternFirstChar = /^[0-9_][a-zA-Z0-9_]{1,14}$/i;
                var message = '';

                if (user_name == "") {
                    message = '';
                } else if (user_name.indexOf("xx", 0) != -1) {
                    message = '为防止被部分防火墙屏蔽，用户名中不能带有 xx';
                } else if (user_name.length < 3) {
                    message = '用户名不能少于3位';
                } else if (user_name.length > 10) {
                    message = '用户名不能大于10位';
                } else if (patternFirstChar.test(user_name)) {
                    message = '用户名必须以字母开头';
                } else if (patternLastChar.test(user_name)) {
                    message = '下划线(_)不能放在末尾';
                } else if (!pattern.test(user_name)) {
                    message = '只能由3-10位字母、数字或下划线(_)构成';
                } else {
                    message = "√";
                }
                return message;
            }
            function checkUserName(user_account, username_explain) {
                var message = checkUserNameInternal(user_account);

                if (message == "") {
                    username_explain.className = "h12";
                } else if (message != "√"){
                    username_explain.style.color = "red";
                } else {
                    username_explain.style.color = "green";
                }
                username_explain.innerHTML = message;
            }
            function userNameFocus(username_explain) {
                username_explain.style.color = "gray";
                username_explain.innerHTML = '用户名由3-10位字母(a-z)、数字(0-9)或下划线(_)构成，并且必须以字母开头';
            }

            //check nickname
            function checkUserNickNameInternal(user_nickname) {
                var nick_name = user_nickname.value.toLowerCase();
                var pattern=/^[a-zA-Z][a-zA-Z0-9_]{1,14}[a-zA-Z0-9]$/i;
                var patternLastChar = /^[a-zA-Z0-9_]{1,15}_$/i;
                var patternFirstChar = /^[0-9_][a-zA-Z0-9_]{1,14}$/i;
                var message = '';

                if (nick_name == "") {
                    message = '';
                } else if (nick_name.indexOf("xx", 0) != -1) {
                    message = '为防止被部分防火墙屏蔽，用户昵称中不能带有 xx';
                } else if (nick_name.length < 3) {
                    message = '用户昵称不能少于3位';
                } else if (nick_name.length > 10) {
                    message = '用户昵称不能大于10位';
                } else if (patternFirstChar.test(nick_name)) {
                    message = '用户昵称必须以字母开头';
                } else if (patternLastChar.test(nick_name)) {
                    message = '下划线(_)不能放在末尾';
                } else if (!pattern.test(nick_name)) {
                    message = '只能由3-10位字母、数字或下划线(_)构成';
                } else {
                    message = "√";
                }
                return message;
            }
            function checkUserNickName(user_nickname, nickname_explain) {
                var message = checkUserNickNameInternal(user_nickname);

                if (message == "") {
                    nickname_explain.className = "h12";
                } else if (message != "√"){
                    nickname_explain.style.color = "red";
                } else {
                    nickname_explain.style.color = "green";
                }
                nickname_explain.innerHTML = message;
            }
            function userNickNameFocus(nickname_explain) {
                nickname_explain.style.color = "gray";
                nickname_explain.innerHTML = '用户昵称由3-10位字母(a-z)、数字(0-9)或下划线(_)构成，并且必须以字母开头';
            }

            //check password
            function checkPasswordInternal(passwordObj) {
                var password = passwordObj.value;
                var numericPattern = /^[0-9]{1,9}$/i;
                var repeatPattern = /^[\w]*(\w)\1{3,}[\w]*$/i;
                var message = '';

                if (password == "") {
                    message = '';
                } else if (password.indexOf(" ", 0) != -1) {
                    message = '密码不能包含空格';
                } else if (password.length < 6) {
                    message = '密码至少要6位以上';
                } else if (numericPattern.test(password)) {
                    message = '不能使用纯数字，这样容易被人猜到';
                } else if (repeatPattern.test(password)) {
                    message = '太多连续或重复的字符(如：1234、abcd、3333、ssss等)';
                } else {
                    message = "√";
                }
                return message;
            }
            function checkPassword(passwordObj, passwordExplainObj) {
                var message = checkPasswordInternal(passwordObj);

                if (message == "") {
                    passwordExplainObj.className = "h12";
                } else if (message != "√"){
                    passwordExplainObj.style.color = "red";
                } else {
                    passwordExplainObj.style.color = "green";
                }
                passwordExplainObj.innerHTML = message;
            }
            function userPasswordFocus(passwordExplainObj) {
                passwordExplainObj.style.color = "gray";
                passwordExplainObj.innerHTML =  '密码要至少6位以上，且不能包含空格，英文字母区分大小写';
            }

            //check password confirm
            function checkPasswordConfirmInternal(password1Obj, password2Obj)
            {
                var password1 = password1Obj.value;
                var password2 = password2Obj.value;
                var message = '';

                if (password2 == "")
                {
                    message = '';
                } else if (password1 != password2) {
                    message = '与上次输入的密码不一致';
                } else {
                    message = "√";
                }
                return message;
            }
            function checkPasswordConfirm(password1Obj, password2Obj, password2ExplainObj)
            {
                var message = checkPasswordConfirmInternal(password1Obj, password2Obj);

                if (message == "") {
                    password2ExplainObj.className = "h12";
                } else if (message != "√"){
                    password2ExplainObj.style.color = "red";
                } else {
                    password2ExplainObj.color = "green";
                }
                password2ExplainObj.innerHTML = message;
            }

            //check register info
            function checkInfo(accountObj, nicknameObj, password1Obj, password2Obj, codeObj) {
                var message = checkUserNameInternal(accountObj);
                if (message != "√"){
                    if (message == "")
                    {
                        alert("用户名不能为空");
                    } else {
                        alert(message);
                    }
                    accountObj.focus();
                    return false;
                }
                message = checkUserNickNameInternal(nicknameObj);
                if (message != "√"){
                    if (message == "")
                    {
                        alert("用户昵称不能为空");
                    } else {
                        alert(message);
                    }
                    nicknameObj.focus();
                    return false;
                }
                message = checkPasswordInternal(password1Obj);
                if (message != "√"){
                    if (message == "")
                    {
                        alert("密码不能为空");
                    } else {
                        alert(message);
                    }
                    password1Obj.focus();
                    return false;
                }
                message = checkPasswordConfirmInternal(password1Obj, password2Obj);
                if (message != "√"){
                    if (message == "")
                    {
                        alert("密码确认不能为空");
                    } else {
                        alert(message);
                    }
                    password2Obj.focus();
                    return false;
                }
                var code = codeObj.value;
                if (code == null || code == ""){
                    alert("请输入验证码!");
                    codeObj.focus();
                    return false;
                }

                return true;
            }

            function submitForm(){
                if (!checkInfo(getObject('User_Account'), getObject('User_NickName'), getObject('User_Password'), getObject('User_RePassword'), getObject('number'))){
                    return false;
                }
                <%= ClientID %>.Submit('<%=ClientID %>', submitFormCallback);
                return false;
            }
            function submitFormCallback(result)
            {
                if(result.error != null && result.error != ""){
                    alert(result.error);
                    return;
                }
                self.location = "<%= SiteUrls.Instance.ControlPanel %>";
            }

        </script>

        <div class="registerwrapper">
            <div class="loginMainTitle">
                <img src="<%= System.Web.Core.Globals.ApplicationPath + "/images/login_title_01.gif" %>">
                蜘蛛侠论坛真诚欢迎您的加入...</div>
            <table class="f12b" cellspacing="0" cellpadding="5" width="100%" border="0">
                <tbody>
                    <tr id="topSpanRow">
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr id="row_user">
                        <td align="right" width="19%" height="24">
                            <ctrl:ResourceLabel ID="ResourceLabel1" runat="Server" CssClass="registerlable" ResourceName="Register_UserName" />
                        </td>
                        <td width="81%">
                            <input type="text" class="bd" id="User_Account" onblur="encodeParse(this);checkUserName(this, getObject('username_explain'));"
                                onfocus="setCurrentItem('row_user'); userNameFocus(getObject('username_explain'));" tabindex="1" maxlength="16" size="13" name="User_Account"
                                style="border-right: #49A9DC 1px solid; border-top: #49A9DC 1px solid; font-size: 9pt;
                                border-left: #49A9DC 1px solid; width: 130px; border-bottom: #49A9DC 1px solid;
                                padding: 2px; background-color: #ffffff" />&nbsp;&nbsp; <span class="h12" id="username_explain">
                                </span>
                        </td>
                    </tr>
                    <tr id="row_nickname">
                        <td align="right" width="19%" height="24">
                            <ctrl:ResourceLabel ID="ResourceLabel2" runat="Server" CssClass="registerlable" ResourceName="Register_NickName" />
                        </td>
                        <td width="81%">
                            <input type="text" class="bd" id="User_NickName" onblur="encodeParse(this);checkUserNickName(this, getObject('nickname_explain'));"
                                onfocus="setCurrentItem('row_nickname'); userNickNameFocus(getObject('nickname_explain'));" tabindex="2" maxlength="16" size="13" name="User_NickName"
                                style="border-right: #49A9DC 1px solid; border-top: #49A9DC 1px solid; font-size: 9pt;
                                border-left: #49A9DC 1px solid; width: 130px; border-bottom: #49A9DC 1px solid;
                                padding: 2px; background-color: #ffffff" />&nbsp;&nbsp; <span class="h12" id="nickname_explain">
                                </span>
                        </td>
                    </tr>
                    <tr id="row_pass">
                        <td align="right" height="24">
                            <ctrl:ResourceLabel ID="ResourceLabel3" runat="Server" CssClass="registerlable" ResourceName="Register_Password" />
                        </td>
                        <td>
                            <input class="bd" onpaste="return false;" id="User_Password" onblur="encodeParse(this);checkPassword(this, getObject('pass_explain'));"
                                onfocus="setCurrentItem('row_pass'); userPasswordFocus(getObject('pass_explain'));" tabindex="3"
                                type="password" size="13" name="User_Password" style="border-right: #49A9DC 1px solid;
                                border-top: #49A9DC 1px solid; font-size: 9pt; border-left: #49A9DC 1px solid;
                                width: 130px; border-bottom: #49A9DC 1px solid; padding: 2px; background-color: #ffffff" />&nbsp;&nbsp;
                            <span class="h12" id="pass_explain"></span>
                        </td>
                    </tr>
                    <tr id="row_pass_confirm">
                        <td align="right" height="24">
                            <span class="registerlable"><ctrl:ResourceLabel ID="ResourceLabel4" runat="Server" CssClass="registerlable" ResourceName="Register_ConfirmPassword" /></span>
                        </td>
                        <td>
                            <input class="bd" onpaste="return false;" id="User_RePassword" onblur="encodeParse(this); checkPasswordConfirm(getObject('User_Password'), this, getObject('pass_confirm_explain'));"
                                onfocus="setCurrentItem('row_pass_confirm'); userPasswordFocus(getObject('pass_confirm_explain'));" tabindex="4"
                                type="password" size="13" name="User_RePassword" style="border-right: #49A9DC 1px solid;
                                border-top: #49A9DC 1px solid; font-size: 9pt; border-left: #49A9DC 1px solid;
                                width: 130px; border-bottom: #49A9DC 1px solid; padding: 2px; background-color: #ffffff" />&nbsp;&nbsp;
                            <span class="h12" id="pass_confirm_explain"></span>
                        </td>
                    </tr>
                    <tr id="row_yzm">
                        <td align="right" height="24">
                            <span class="registerlable"><ctrl:ResourceLabel ID="ResourceLabel5" runat="Server" CssClass="registerlable" ResourceName="Register_AuthCode" /></span>
                        </td>
                        <td valign="middle">
                            <input id="identifier" type="hidden" value="1211895332816676" name="identifier">
                            <div>
                                <input type="text" id="number" onblur="encodeParse(this);"
                                    onfocus="setCurrentItem('row_yzm');;"
                                    style="border-right: #49A9DC 1px solid;
                                    float: left; border-top: #49A9DC 1px solid; font-size: 9pt; border-left: #49A9DC 1px solid;
                                    width: 40px; border-bottom: #49A9DC 1px solid; padding: 2px; background-color: #ffffff"
                                    tabindex="5" maxlength="4" size="4" name="number" />
                                <img id="yzm" style="border-right: #000 1px solid; border-top: #000 1px solid; float: left;
                                    margin-left: 10px; border-left: #000 1px solid; border-bottom: #000 1px solid" height="18"
                                    onclick="return changeAuthCode();" src="authCode.ashx" align="bottom">&nbsp;&nbsp;<a
                                        style="font-size: 12px; color: #0A6FA5;" onclick="return changeAuthCode();" href="javascript:void(0);">点击换一个!</a>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="43">
                            &nbsp;
                        </td>
                        <td>
                            <ctrl:ResourceButton ID="ResourceButton1" CssClass="btn3" OnClientClick="return submitForm()" runat="server" tabindex="6" />
                        </td>
                    </tr>
                    <tr id="bottomSpan">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <ctrl:Footer runat="server" />
    </form>
</body>
</html>
