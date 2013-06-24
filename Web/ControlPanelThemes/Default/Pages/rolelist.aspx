<%@ Page Language="C#" Inherits="Forum.Business.RoleListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script language="javascript" type="text/javascript">

function deleteRole(roleId){
    var bool = confirm("确认删除该角色吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.DeleteRole('<%= ClientID %>', roleId, refreshPage);
}

function deleteRoles(){

    var check = document.getElementsByName("roleId");
    var ifSelect;
    var roleIds = '';
    for(var i=0;i<check.length;i++){
        if(check[i].checked==true){
            if(roleIds == '')
                roleIds = check[i].value;
            else
                roleIds = roleIds + ":" + check[i].value;
            ifSelect = true;
        }
    }
    if(ifSelect == true){
        var delSelect = confirm("确认删除这些角色吗？");
        if(delSelect == false){
            return;
        }
        <%= ClientID %>.DeleteRoles('<%= ClientID %>', roleIds, refreshPage);
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
        var check = document.getElementsByName("roleId");
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
    var check = document.getElementsByName("roleId");
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

</script>

<div class="AdminArea">
    <fieldset>
        <legend>角色管理</legend>

        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="全选/取消全选" /></th>
                            <th class="TitleCol">角色名称</th>
                            <th class="ActionCol">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="NumCol">
                        <input type="checkbox" onclick="updateCheckAllChecked(this)" name="roleId" id="roleId" value="<%# ((System.Web.Core.Role)Container.DataItem).EntityId.Value %>" />
                    </td>
                    <td class="TitleCol">
                        <a href="<%# SiteUrls.Instance.GetRoleEditUrl(((System.Web.Core.Role)Container.DataItem).EntityId.Value) %>"><%# ((System.Web.Core.Role)Container.DataItem).Name%></a>
                    </td>
                    <td class="ActionCol">
                        <a class="" href="javascript:void(0);" onclick="deleteRole(<%# ((System.Web.Core.Role)Container.DataItem).EntityId.Value %>)" alt="删除" title="删除"><span>删除</span></a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
                <div class="FormRow ButtonRow">
                    <input type="button" onclick="deleteRoles()" value="删除选择"/>&nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="button" onclick="<%= "window.location.href='" + SiteUrls.Instance.GetRoleAddUrl() + "';" %>" value="添加角色"/>
                </div>
            </FooterTemplate>
        </ctrl:NoneStateRepeater>
        <div id="pagingWrapper">
            <asp:Panel ID="Panel1" Runat="server" align="right" CssClass="CommonPagingArea">
                <ctrl:CurrentPage Cssclass="columnText" id="currentPage" runat="server" />
                <ctrl:Pager id="pager" runat="server" PageSize="20" />
            </asp:Panel>
        </div>
    </fieldset>
</div>

</asp:Content>