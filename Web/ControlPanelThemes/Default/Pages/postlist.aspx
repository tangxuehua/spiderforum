<%@ Page Language="C#" Inherits="Forum.Business.PostListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script language="javascript" type="text/javascript">

function deletePost(postId){
    var bool = confirm("ȷ��ɾ���ûظ���");
    if(bool == false){
        return;
    }
    <%= ClientID %>.DeletePost('<%= ClientID %>', postId, refreshPage);
}

function deletePosts(){

    var check = document.getElementsByName("postId");
    var ifSelect;
    var postIds = '';
    for(var i=0;i<check.length;i++){
        if(check[i].checked==true){
            if(postIds == '')
                postIds = check[i].value;
            else
                postIds = postIds + ":" + check[i].value;
            ifSelect = true;
        }
    }
    if(ifSelect == true){
        var delSelect = confirm("ȷ��ɾ����Щ�ظ���");
        if(delSelect == false){
            return;
        }
        <%= ClientID %>.DeletePosts('<%= ClientID %>', postIds, refreshPage);
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
        var check = document.getElementsByName("postId");
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
    var check = document.getElementsByName("postId");
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

<div class="AdminArea">
    <fieldset>
        <legend>�ظ�����</legend>    
                
        <ctrl:NoneStateRepeater id="list" runat="server">
            <HeaderTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                    <thead>
                        <tr>
                            <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="ȫѡ/ȡ��ȫѡ" /></th>
                            <th class="TitleCol">�ظ�����</th>
                            <th class="ActionCol">����</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="NumCol">
                        <input type="checkbox" onclick="updateCheckAllChecked(this)" name="postId" id="postId" value="<%# ((Post)Container.DataItem).EntityId.Value %>" />
                    </td>
                    <td class="TitleCol">
                        <%# ((Post)Container.DataItem).Body.Value %>
                    </td>
                    <td class="ActionCol">
                        <a class="" target="_blank" href="<%# SiteUrls.Instance.GetPostEditUrl(((Post)Container.DataItem).EntityId.Value) %>" alt="�༭" title="�༭"><span>�༭</span></a>
                        <a class="" href="javascript:void(0);" onclick="deletePost(<%# ((Post)Container.DataItem).EntityId.Value %>)" alt="ɾ��" title="ɾ��"><span>ɾ��</span></a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
                <div class="FormRow ButtonRow">
                    <input type="button" onclick="deletePosts()" value="ɾ��ѡ��"/>&nbsp;&nbsp;&nbsp;&nbsp;
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