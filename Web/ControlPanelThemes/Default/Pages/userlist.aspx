<%@ Page Language="C#" Inherits="Forum.Business.UserListPage, Forum.Business" MasterPageFile="~/controlpanelthemes/default/masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="System.Web.Core" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script language="javascript" type="text/javascript">

function resetUserPassword(userId){
    var bool = confirm("确认重置该用户密码吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.ResetUserPassword('<%= ClientID %>', userId, ResetPasswordCallBack);
}
function lockUser(userId){
    var bool = confirm("确认冻结该用户吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.LockUser('<%= ClientID %>', userId, LockUserCallBack);
}
function unLockUser(userId){
    var bool = confirm("确认解冻该用户吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.UnLockUser('<%= ClientID %>', userId, UnLockUserCallBack);
}

function deleteUser(userId){
    var bool = confirm("确认删除该用户吗？");
    if(bool == false){
        return;
    }
    <%= ClientID %>.DeleteUser('<%= ClientID %>', userId, DeleteUserCallBack);
}

function deleteUsers(){

    var check = document.getElementsByName("userId");
    var ifSelect;
    var userIds = '';
    for(var i=0;i<check.length;i++){
        if(check[i].checked==true){
            if(userIds == '')
                userIds = check[i].value;
            else
                userIds = userIds + ":" + check[i].value;
            ifSelect = true;
        }
    }
    if(ifSelect == true){
        var delSelect = confirm("确认删除这些用户吗？");
        if(delSelect == false){
            return;
        }
        <%= ClientID %>.DeleteUsers('<%= ClientID %>', userIds, DeleteUserCallBack);
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
        var check = document.getElementsByName("userId");
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
    var check = document.getElementsByName("userId");
    for(var i=0;i<check.length;i++){
        check[i].checked=checkAll.checked;
    }
}

function ResetPasswordCallBack(res)
{
    if(res.error != null && res.error != "")
    {
        alert(res.error);
        return false;
    }
    alert("密码重置成功，新密码是：" + res.value);
}
function LockUserCallBack(res)
{
    if(res.error != null && res.error != "")
    {
        alert(res.error);
        return false;
    }
    window.location.reload();
    var checkAll = document.getElementById("CheckAll");
    checkAll.checked = false;
    toggleAllCheck(checkAll);
}
function UnLockUserCallBack(res)
{
    if(res.error != null && res.error != "")
    {
        alert(res.error);
        return false;
    }
    window.location.reload();
    var checkAll = document.getElementById("CheckAll");
    checkAll.checked = false;
    toggleAllCheck(checkAll);
}
function DeleteUserCallBack(res)
{
    if(res.error != null && res.error != "")
    {
        alert(res.error);
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
        <legend>用户管理</legend>

        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="全选/取消全选" /></th>
                            <th class="TitleCol">用户昵称</th>
                            <th class="BigActionCol">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="NumCol">
                        <input type="checkbox" onclick="updateCheckAllChecked(this)" name="userId" id="userId" value="<%# ((ForumUser)Container.DataItem).EntityId.Value %>" />
                    </td>
                    <td class="TitleCol">
                        <%# ((ForumUser)Container.DataItem).NickName.Value %>
                    </td>
                    <td class="BigActionCol">
                        <a target="_blank" href="<%# SiteUrls.Instance.GetUserRolesUrl(((ForumUser)Container.DataItem).EntityId.Value) %>" title="管理当前用户的角色"><span>角色</span></a>
                        <a href="javascript:void(0);" style="display:<%# ((ForumUser)Container.DataItem).UserStatus.Value == (int)UserStatus.Locked ? "none" : "" %>;" onclick="lockUser(<%# ((ForumUser)Container.DataItem).EntityId.Value %>)" alt="冻结帐号" title="冻结当前用户的帐号"><span>冻结</span></a>
                        <a href="javascript:void(0);" style="display:<%# ((ForumUser)Container.DataItem).UserStatus.Value == (int)UserStatus.Normal ? "none" : "" %>;color:<%# ((ForumUser)Container.DataItem).UserStatus.Value == (int)UserStatus.Normal ? "#000000" : "#FF0000" %>" onclick="unLockUser(<%# ((ForumUser)Container.DataItem).EntityId.Value %>)" alt="解冻帐号" title="解冻当前用户的帐号"><span>解冻</span></a>
                        <a href="javascript:void(0);" onclick="resetUserPassword(<%# ((ForumUser)Container.DataItem).EntityId.Value %>)" title="重置当前用户的密码"><span>重置密码</span></a>
                        <a href="javascript:void(0);" onclick="deleteUser(<%# ((ForumUser)Container.DataItem).EntityId.Value %>)" title="删除"><span>删除</span></a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
                <div class="FormRow ButtonRow">
                    <input type="button" class="Button" onclick="deleteUsers()" value="删除选择"/>&nbsp;&nbsp;&nbsp;&nbsp;
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
