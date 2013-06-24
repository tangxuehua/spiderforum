<%@ Page Language="C#" Inherits="Forum.Business.RoleListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script language="javascript" type="text/javascript">

function deleteRole(roleId){
    var bool = confirm("ȷ��ɾ���ý�ɫ��");
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
        var delSelect = confirm("ȷ��ɾ����Щ��ɫ��");
        if(delSelect == false){
            return;
        }
        <%= ClientID %>.DeleteRoles('<%= ClientID %>', roleIds, refreshPage);
    }
    else{
        alert("��ѡ��Ҫɾ���ļ�¼��");
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
        <legend>��ɫ����</legend>

        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="ȫѡ/ȡ��ȫѡ" /></th>
                            <th class="TitleCol">��ɫ����</th>
                            <th class="ActionCol">����</th>
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
                        <a class="" href="javascript:void(0);" onclick="deleteRole(<%# ((System.Web.Core.Role)Container.DataItem).EntityId.Value %>)" alt="ɾ��" title="ɾ��"><span>ɾ��</span></a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
                <div class="FormRow ButtonRow">
                    <input type="button" onclick="deleteRoles()" value="ɾ��ѡ��"/>&nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="button" onclick="<%= "window.location.href='" + SiteUrls.Instance.GetRoleAddUrl() + "';" %>" value="��ӽ�ɫ"/>
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