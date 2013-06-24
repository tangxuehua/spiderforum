<%@Register tagPrefix="ctrl" tagName="AnonymousUserControl" src="~/themes/default/controls/Skin-AnonymousUserControl.ascx" %>
<%@Register tagPrefix="ctrl" tagName="RegisteredUserControl" src="~/themes/default/controls/Skin-RegisteredUserControl.ascx" %>
<%@ Import Namespace="Forum.Business" %>

<div class="head090114">
    <p class="logo"><a href="<%= SiteUrls.Instance.Home %>"><img src="img.ashx?fileurl=images/logo_bbs.gif" alt="" border="0" /></a></p>
    <p class="SiteDescription">��վרע�����ۣ���������������ƣ�OOA\D��������������ƣ�DDD�����������</p>
    <div class="rInfo">
        <p>
            <ctrl:LoginView runat="server">
                <AnonymousTemplate>
                    <ctrl:AnonymousUserControl runat="server" />
                </AnonymousTemplate>
                <LoggedInTemplate>
                    <ctrl:RegisteredUserControl runat="server" />
                </LoggedInTemplate>
            </ctrl:LoginView>
        </p>
        <p><span>&nbsp;</span></p>
    </div>
</div>