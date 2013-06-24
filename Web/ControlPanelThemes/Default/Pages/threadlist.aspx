<%@ Page Language="C#" Inherits="Forum.Business.ThreadListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script type="text/javascript">

function deleteThread(articleId){
    var bool = confirm("确认删除该帖子吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.DeleteThread('<%= ClientID %>', articleId, refreshPage);
}

function deleteEntities(){

    var check = document.getElementsByName("articleId");
    var ifSelect;
    var articleIds = '';
    for(var i=0;i<check.length;i++){
        if(check[i].checked==true){
            if(articleIds == '')
                articleIds = check[i].value;
            else
                articleIds = articleIds + ":" + check[i].value;
            ifSelect = true;
        }
    }
    if(ifSelect == true){
        var delSelect = confirm("确认删除这些帖子吗？");
        if(delSelect == false){
            return;
        }
        <%= ClientID %>.DeleteThreads('<%= ClientID %>', articleIds, refreshPage);
    }
    else{
        alert("请选择要删除的记录！");
    }
}

function updateCheckAllChecked(check){
    var checkAll = document.getElementById("CheckAll");
    if(!check.checked){
        checkAll.checked = false;
    }
    else
    {
        var ifAllSelect = true;
        var check = document.getElementsByName("articleId");
        for(var i=0;i<check.length;i++){
            if(check[i].checked==false){
                ifAllSelect = false;
            }
        }
        if(ifAllSelect == true){
            checkAll.checked = true;
        }
    }
}

function toggleAllCheck(checkAll){
    var check = document.getElementsByName("articleId");
    for(var i=0;i<check.length;i++){
        check[i].checked=checkAll.checked;
    }
}

function refreshPage(res)
{
    window.location.reload();
    var checkAll = document.getElementById("CheckAll");
    checkAll.checked = false;
    toggleAllCheck(checkAll);
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

</script>

<div class="AdminArea">
    <fieldset>
        <legend>帖子管理</legend>
        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="全选/取消全选" /></th>
                            <th class="TitleCol">标题</th>
                            <th class="IsRecommendedCol">帖子状态</th>
                            <th class="CountCol">回复总数</th>
                            <th class="ActionCol">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="NumCol">
                        <input type="checkbox" onclick="updateCheckAllChecked(this)" name="articleId" id="articleId" value="<%# ((Thread)Container.DataItem).EntityId.Value %>" />
                    </td>
                    <td class="TitleCol">
                        <a href="<%# SiteUrls.Instance.GetThreadEditUrl(((Thread)Container.DataItem).EntityId.Value) %>"><%# Page.Server.HtmlEncode(((Thread)Container.DataItem).Subject.Value)%></a>
                    </td>
                    <td class='<%# GetThreadStatus((Thread)Container.DataItem) %>'>
                    &nbsp;
                    </td>
                    <td class="CountCol">
                        <a target="_blank" href="<%# SiteUrls.Instance.GetPostListUrl(((Thread)Container.DataItem).EntityId.Value) %>" title="回复管理"><%# ((Thread)Container.DataItem).TotalPosts.Value%></a>
                    </td>
                    <td class="ActionCol">
                        <a class="" href="<%# SiteUrls.Instance.GetThreadEditUrl(((Thread)Container.DataItem).EntityId.Value) %>" alt="编辑" title="编辑"><span>编辑</span></a>
                        <a class="" href="javascript:void(0);" onclick="deleteThread(<%# ((Thread)Container.DataItem).EntityId.Value %>)" alt="删除" title="删除"><span>删除</span></a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
                <div class="FormRow ButtonRow">
                    <input type="button" onclick="deleteEntities()" value="删除选择"/>&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
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