<%@ Page Language="C#" Inherits="Forum.Business.GroupListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>
<%@ Import Namespace="System.Web.Core" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script language="javascript" type="text/javascript">
    function deleteGroup(groupId){
        var bool = confirm("ȷ��ɾ������̳�������");
        if(bool == false){
            return;
        }
        <%= ClientID %>.DeleteGroup('<%= ClientID %>', groupId, refreshPage);
    }

    function deleteGroups(){

        var check = document.getElementsByName("groupId");
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
            var delSelect = confirm("ȷ��ɾ����Щ��̳�������");
            if(delSelect == false){
                return;
            }
            <%= ClientID %>.DeleteGroups('<%= ClientID %>', groupIds, refreshPage);
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
            var check = document.getElementsByName("groupId");
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
        var check = document.getElementsByName("groupId");
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

    function TurnPage(pageIndex)
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
        <legend>��������</legend>    
        <div id="listWrapper">
            <ctrl:NoneStateRepeater id="list" runat="server">
                <HeaderTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                        <thead>
                            <tr>
                                <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="ȫѡ/ȡ��ȫѡ" /></th>
                                <th class="TitleCol">���������</th>
                                <th class="EnabledCol">�Ƿ�����</th>
                                <th class="ActionCol">����</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="NumCol">
                            <input type="checkbox" onclick="updateCheckAllChecked(this)" name="groupId" id="groupId" value="<%# ((Forum.Business.Group)Container.DataItem).EntityId.Value %>" />
                        </td>
                        <td class="TitleCol">
                            <a href='<%# SiteUrls.Instance.GetGroupEditUrl(((Forum.Business.Group)Container.DataItem).EntityId.Value) %>'><%# ((Forum.Business.Group)Container.DataItem).Subject.Value%></a>
                        </td>
                        <td class="EnabledCol">
                            <%# ((Forum.Business.Group)Container.DataItem).Enabled.Value == 1 ? "��" : "��"%>
                        </td>
                        <td class="ActionCol">
                            <a class="" href="javascript:void(0);" onclick="deleteGroup(<%# ((Forum.Business.Group)Container.DataItem).EntityId.Value %>)" alt="ɾ��" title="ɾ��"><span>ɾ��</span></a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                    <div class="FormRow ButtonRow">
                        <input type="button" onclick="deleteGroups()" value="ɾ��ѡ��"/>&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="button" onclick='window.location.href="<%= SiteUrls.Instance.GetGroupAddUrl() %>"' value="��Ӱ����"/>
                    </div>
                </FooterTemplate>
            </ctrl:NoneStateRepeater>
        </div>
        <div id="pagingWrapper">
            <ctrl:CurrentPage id="currentPage" runat="server" />
            <ctrl:AjaxPager id="pager" runat="server" PageSize="20" TurnPageClientFunction="TurnPage" />
        </div>
    </fieldset>
</div>

</asp:Content>