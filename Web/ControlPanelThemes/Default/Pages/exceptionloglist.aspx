<%@ Page Language="C#" Inherits="Forum.Business.ExceptionLogListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>
<%@ Import Namespace="System.Web.Core" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

    <script language="javascript" type="text/javascript">
        function deleteExceptionLog(groupId){
            var bool = confirm("确认删除该错误日志吗？");
            if(bool == false){
                return;
            }
            <%= ClientID %>.DeleteExceptionLog('<%= ClientID %>', groupId, refreshPage);
        }

        function deleteExceptionLogs(){

            var check = document.getElementsByName("entityId");
            var ifSelect;
            var groupIds = '';
            for(var i=0;i<check.length;i++){
                if(check[i].checked==true){
                    if(groupIds == '')
                        groupIds = check[i].value;
                    else
                        groupIds = groupIds + ":" + check[i].value;
                    ifSelect = true;
                }
            }
            if(ifSelect == true){
                var delSelect = confirm("确认删除这些错误日志吗？");
                if(delSelect == false){
                    return;
                }
                <%= ClientID %>.DeleteExceptionLogs('<%= ClientID %>', groupIds, refreshPage);
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
                var check = document.getElementsByName("entityId");
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
            var check = document.getElementsByName("entityId");
            for(var i=0;i<check.length;i++){
                check[i].checked=checkAll.checked;
            }
        }

        function refreshPage(result)
        {
            if(result.error != null && result.error != "")
            {
                alert(result.error);
                return false;
            }
            window.location.reload();
            var checkAll = document.getElementById("CheckAll");
            checkAll.checked = false;
            toggleAllCheck(checkAll);
        }

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
            document.getElementById('pagingWrapper').innerHTML = result.value.PagingContent;
            return true;
        }

    </script>

    <div class="AdminArea">
        <fieldset>
            <legend>错误日志管理</legend>    
            <div id="listWrapper">
                <ctrl:NoneStateRepeater id="list" runat="server">
                    <HeaderTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                            <thead>
                                <tr>
                                    <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="全选/取消全选" /></th>
                                    <th class="TitleCol">错误信息标题</th>
                                    <th class="EnabledCol">发生频率</th>
                                    <th class="DateLastOccurred">最后发生时间</th>
                                    <th class="ActionCol">操作</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="NumCol">
                                <input type="checkbox" onclick="updateCheckAllChecked(this)" name="entityId" id="entityId" value="<%# ((ExceptionLog)Container.DataItem).EntityId.Value %>" />
                            </td>
                            <td class="TitleCol">
                                <a target="_blank" href="<%# SiteUrls.Instance.GetViewExceptionLogUrl(((ExceptionLog)Container.DataItem).EntityId.Value) %>"><%# ((ExceptionLog)Container.DataItem).Message.Value %></a>
                            </td>
                            <td class="EnabledCol">
                                <%# ((ExceptionLog)Container.DataItem).Frequency.Value %>
                            </td>
                            <td class="DateLastOccurred">
                                <%# ((ExceptionLog)Container.DataItem).DateLastOccurred.Value%>
                            </td>
                            <td class="ActionCol">
                                <a class="" href="javascript:void(0);" onclick="deleteExceptionLog(<%# ((ExceptionLog)Container.DataItem).EntityId.Value %>)" alt="删除" title="删除"><span>删除</span></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                        <div class="FormRow ButtonRow">
                            <input type="button" onclick="deleteExceptionLogs()" value="删除选择"/>&nbsp;&nbsp;&nbsp;&nbsp;
                        </div>
                    </FooterTemplate>
                </ctrl:NoneStateRepeater>
            </div>
            <div id="pagingWrapper">
                <ctrl:CurrentPage id="currentPage" runat="server" />
                <ctrl:AjaxPager id="pager" runat="server" PageSize="20" TurnPageClientFunction="turnPage" />
            </div>
        </fieldset>
    </div>

</asp:Content>