<%@ Page Language="C#" Inherits="Forum.Business.MyCloseThreadListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script runat="server" type="text/C#">
    private string GetThreadStatus(Thread thread)
    {
        if (thread.ThreadStatus.Value == (int)ThreadStatus.Normal)
        {
            return "PrefixCol List";
        }
        else if (thread.ThreadStatus.Value == (int)ThreadStatus.Recommended)
        {
            return "PrefixCol Jian";
        }
        else if (thread.ThreadStatus.Value == (int)ThreadStatus.Stick)
        {
            return "PrefixCol Ding";
        }
        return "PrefixCol List";
    }

</script>

<div class="AdminArea">
    <fieldset>
        <legend>我已结的帖子</legend>
        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="TitleCol">标题</th>
                            <th class="IsRecommendedCol">帖子状态</th>
                            <th class="CountCol">回复总数</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="TitleCol">
                        <a target="_blank" href="<%# SiteUrls.Instance.GetThreadUrl(((Thread)Container.DataItem).EntityId.Value) %>"><%# Page.Server.HtmlEncode(((Thread)Container.DataItem).Subject.Value)%></a>
                    </td>
                    <td class='<%# GetThreadStatus((Thread)Container.DataItem) %>'>&nbsp;
                    </td>
                    <td class="CountCol">
                        <%# ((Thread)Container.DataItem).TotalPosts.Value%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </ctrl:NoneStateRepeater>
        <div id="pagingWrapper">
            <asp:Panel ID="Panel1" Runat="server" align="right" CssClass="CommonPagingArea">
                <ctrl:CurrentPage Cssclass="columnText" id="currentPage" runat="server" />
                <ctrl:Pager id="pager" runat="server" PageSize="50" />
            </asp:Panel>
        </div>
    </fieldset>
</div>

</asp:Content>