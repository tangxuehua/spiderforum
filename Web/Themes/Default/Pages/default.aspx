<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="Forum.Business.DefaultPage, Forum.Business" MasterPageFile="~/Themes/Default/Masters/Master.master" %>
<%@ Import Namespace="Forum.Business" %>
<%@ Import Namespace="System.Web.Core" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <ctrl:HeadControl runat="server" TitleResourceName="PageTitle_Home" />

    <script language="javascript" type="text/javascript">
        function turnPage(pageIndex)
        {
            <%= ClientID %>.RefreshList('<%= ClientID %>', pageIndex, RefreshScreen);
        }
        function RefreshScreen(result)
        {
            if(result.error != null && result.error != "")
            {
                alert(result.error);
                return false;
            }
            document.getElementById('listWrapper').innerHTML = result.value.ListContent;
            document.getElementById('pagingWrapper1').innerHTML = result.value.PagingContent;
            document.getElementById('pagingWrapper2').innerHTML = result.value.PagingContent;
            return true;
        }
    </script>

    <script runat="server" type="text/C#">
        private string GetThreadStatus(Thread thread)
        {
            if (thread.ThreadStatus.Value == (int)ThreadStatus.Normal)
            {
                return "List";
            }
            else if (thread.ThreadStatus.Value == (int)ThreadStatus.Recommended)
            {
                return "Jian";
            }
            else if (thread.ThreadStatus.Value == (int)ThreadStatus.Stick)
            {
                return "Ding";
            }
            return "List";
        }
        private string GetAddButtonUrl(int sectionId)
        {
            if (sectionId > 0)
            {
                return SiteUrls.Instance.GetAddThreadUrl(sectionId);
            }
            else
            {
                return "javascript:alert('请先选择一个要发帖的版块。');";
            }
        }
    </script>

    <div class="guide" style="padding-top: 15px;">
        <ctrl:ThreadBreadCrumb runat="server" />
    </div>
    <div class="tag">
        <ul>
            <li class="<%= GetValue<int>(ForumParameterName.ThreadStatus) != (int)ThreadStatus.Recommended ? "at" : "" %>"><span class="<%= GetValue<int>(ForumParameterName.ThreadStatus) != (int)ThreadStatus.Recommended ? "" : "noneVisible" %>">所有帖子</span><span class="<%= GetValue<int>(ForumParameterName.ThreadStatus) != (int)ThreadStatus.Recommended ? "noneVisible" : "tagSpan" %>"><a href="<%= SiteUrls.Instance.GetSectionThreadsUrl(SectionId) %>"><strong>&nbsp;所有帖子&nbsp;</strong></a></span></li>
            <li class="<%= GetValue<int>(ForumParameterName.ThreadStatus) == (int)ThreadStatus.Recommended ? "at" : "" %>"><span class="<%= GetValue<int>(ForumParameterName.ThreadStatus) == (int)ThreadStatus.Recommended ? "" : "noneVisible" %>">推荐帖子</span><span class="<%= GetValue<int>(ForumParameterName.ThreadStatus) == (int)ThreadStatus.Recommended ? "noneVisible" : "tagSpan" %>"><a href="<%= SiteUrls.Instance.GetSectionRecommendedThreadsUrl(SectionId) %>"><strong>&nbsp;推荐帖子&nbsp;</strong></a></span></li>
        </ul>
        <a href="<%= GetAddButtonUrl(SectionId) %>"><img src="img.ashx?fileurl=images/btn522.gif" alt="" width="77" height="23" border="0" class="fl" /></a>
        <div class="y1">
        </div>
        <div id="pagingWrapper1" class="pageBox">
            <ctrl:AjaxPager ID="pagerUp" PageLength="11" TurnPageClientFunction="turnPage" PageSize="30" runat="server" />
        </div>
    </div>
    <div id="listWrapper" class="table02">
        <ctrl:NoneStateRepeater ID="list" runat="server">
            <HeaderTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <td width="2%" align="center">&nbsp;</td>
                            <td width="66%">帖子标题</td>
                            <td width="8%" align="center">点击/回复</td>
                            <td width="7%" align="center">更新时间</td>
                            <td width="9%" align="center">发帖人</td>
                            <td width="8%" align="center">回复人</td>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="left" class="<%# GetThreadStatus((Thread)Container.DataItem) %>">&nbsp;</td>
                    <td class="f14">
                        <a target="_blank" href="<%# SiteUrls.Instance.GetThreadUrl(((Thread)Container.DataItem).EntityId.Value) %>" title="<%# Page.Server.HtmlEncode(((Thread)Container.DataItem).Subject.Value)%>"><%# Page.Server.HtmlEncode(((Thread)Container.DataItem).Subject.Value)%></a>
                    </td>
                    <td align="center">
                        <%# ((Thread)Container.DataItem).TotalViews.Value%>/<%# ((Thread)Container.DataItem).TotalPosts.Value%></td>
                    <td class="gray" align="center">
                        <%# DataBinder.Eval(((Thread)Container.DataItem).UpdateDate, "Value", "{0:MM-dd HH:mm}")%>
                    </td>
                    <td align="center">
                        <%# ((Thread)Container.DataItem).Author.Value%>
                    </td>
                    <td align="center">
                        <%# string.IsNullOrEmpty(((Thread)Container.DataItem).MostRecentReplierName.Value) ? "-" : ((Thread)Container.DataItem).MostRecentReplierName.Value%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </ctrl:NoneStateRepeater>
    </div>
    <div class="m_page">
        <div id="pagingWrapper2" class="pageBox">
            <ctrl:AjaxPager ID="pagerDown" PageLength="11" TurnPageClientFunction="turnPage" PageSize="30" runat="server" />
        </div>
    </div>
    <div class="blank50">&nbsp;</div>
</asp:Content>