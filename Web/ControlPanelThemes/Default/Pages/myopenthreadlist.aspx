<%@ Page Language="C#" Inherits="Forum.Business.MyOpenThreadListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script type="text/javascript">

function deleteThread(threadId){
    var bool = confirm("确认删除该帖子吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.DeleteThread('<%= ClientID %>', threadId, refreshPage);
}

function refreshPage(res)
{
    window.location.reload();
}

</script>

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
    private string GetTarget(Thread thread)
    {
        if (CanEditMyThread)
        {
            return "_self";
        }
        else
        {
            return "_blank";
        }
    }
    private string GetSubjectLinkUrl(Thread thread)
    {
        if (CanEditMyThread)
        {
            return SiteUrls.Instance.GetMyThreadEditUrl(thread.EntityId.Value);
        }
        else
        {
            return SiteUrls.Instance.GetThreadUrl(thread.EntityId.Value);
        }
    }
    private string GetDeleteHrefClass()
    {
        if (CanDeleteMyThread)
        {
            return "";
        }
        else
        {
            return "nonedisplay";
        }
    }
</script>

<div class="AdminArea">
    <fieldset>
        <legend>我未结的帖子</legend>
                
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
                        <a target="<%# GetTarget((Thread)Container.DataItem) %>" href="<%# GetSubjectLinkUrl((Thread)Container.DataItem) %>"><%# Page.Server.HtmlEncode(((Thread)Container.DataItem).Subject.Value)%></a>
                    </td>
                    <td class='<%# GetThreadStatus((Thread)Container.DataItem) %>'>&nbsp;
                    </td>
                    <td class="CountCol">
                        <%# ((Thread)Container.DataItem).TotalPosts.Value%>
                        <a class="<%# GetDeleteHrefClass() %>" href="javascript:void(0);" onclick="deleteThread(<%# ((Thread)Container.DataItem).EntityId.Value %>)" title="删除"><span>删除</span></a>
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