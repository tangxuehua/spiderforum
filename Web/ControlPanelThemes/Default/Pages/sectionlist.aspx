<%@ Page Language="C#" Inherits="Forum.Business.SectionListPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelIFrameContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<script language="javascript" type="text/javascript">

    function deleteSection(sectionId){
        var bool = confirm("ȷ��ɾ���ð����");
        if(bool == false){
            return;
        }
        <%= ClientID %>.DeleteSection('<%= ClientID %>', sectionId, refreshPage);
    }

    function deleteSections(){

        var check = document.getElementsByName("sectionId");
        var ifSelect;
        var articleCategoryIds = '';
        for(var i=0;i<check.length;i++){
            if(check[i].checked==true){
                if(articleCategoryIds == '')
                    articleCategoryIds = check[i].value;
                else
                    articleCategoryIds = articleCategoryIds + ":" + check[i].value;
                ifSelect = true;
            }
        }
        if(ifSelect == true){
            var delSelect = confirm("ȷ��ɾ����Щ�����");
            if(delSelect == false){
                return;
            }
            <%= ClientID %>.DeleteSections('<%= ClientID %>', articleCategoryIds, refreshPage);
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
            var check = document.getElementsByName("articleCategoryId");
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
        var check = document.getElementsByName("articleCategoryId");
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

    function addSection()
    {
        window.location.href='<%= SiteUrls.Instance.GetSectionAddUrl(0) %>';
    }
    
    function RefreshList()
    {           
	    <%= ClientID %>.RefreshList('<%= ClientID %>', document.getElementById('<%= groupDropDownList.ClientID %>').value, 1, RefreshListCallBack);
    }
    function RefreshListCallBack(result)
    {
        if(RefreshScreen(result) == true)
        {
            SaveConditions();
        }
    }
    function SaveConditions()
    {
        document.getElementById('previousGroupIdHidden').value = document.getElementById('<%= groupDropDownList.ClientID %>').value;
    }

    function TurnPage(pageIndex)
    {
	    <%= ClientID %>.RefreshList('<%= ClientID %>', document.getElementById('previousGroupIdHidden').value, pageIndex, RefreshScreen);
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

<div class="TopArea">
    ѡ�����飺<ctrl:ValuedDropDownList ID="groupDropDownList" runat="server" Width="150px" onChange="RefreshList()" />
</div>
<div class="AdminArea">
    <fieldset>
        <legend>������</legend>
        <div id="listWrapper">
            <ctrl:NoneStateRepeater id="list" runat="server">
                <HeaderTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" class="AdminTable">
                        <thead>
                            <tr>
                                <th class="NumCol"><input type="checkbox" onclick="toggleAllCheck(this)" id="CheckAll" title="ȫѡ/ȡ��ȫѡ" /></th>
                                <th class="TitleCol">�������</th>
                                <th class="EnabledCol">�Ƿ�����</th>
                                <th class="CountCol">��������</th>
                                <th class="BigActionCol">����</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="NumCol">
                            <input type="checkbox" onclick="updateCheckAllChecked(this)" name="articleCategoryId" id="sectionId" value="<%# ((Section)Container.DataItem).EntityId.Value %>" />
                        </td>
                        <td class="TitleCol">
                            <a href="<%# SiteUrls.Instance.GetSectionEditUrl(((Section)Container.DataItem).EntityId.Value) %>"><%# ((Section)Container.DataItem).Subject.Value%></a>
                        </td>
                        <td class="EnabledCol">
                            <%# ((Section)Container.DataItem).Enabled.Value == 1 ? "��" : "��"%>
                        </td>
                        <td class="CountCol">
                            <a target="_blank" href="<%# SiteUrls.Instance.GetThreadListUrl(((Section)Container.DataItem).EntityId.Value) %>"><%# ((Section)Container.DataItem).TotalThreads %></a>
                        </td>
                        <td class="BigActionCol">
                            <a target="_blank" href="<%# SiteUrls.Instance.GetSectionAdminsUrl(((Section)Container.DataItem).EntityId.Value, int.Parse(AdminUserRoleId)) %>">����</a>
                            <a class="" href="<%# SiteUrls.Instance.GetSectionEditUrl(((Section)Container.DataItem).EntityId.Value) %>" alt="�༭" title="�༭"><span>�༭</span></a>
                            <a class="" href="javascript:void(0);" onclick="deleteSection(<%# ((Section)Container.DataItem).EntityId.Value %>)" alt="ɾ��" title="ɾ��"><span>ɾ��</span></a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                    <div class="FormRow ButtonRow">
                        <input type="button" onclick="deleteSections()" value="ɾ��ѡ��"/>&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="button" onclick="addSection()" value="��Ӱ��"/>
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

<input type="hidden" id="previousGroupIdHidden" value="0" />

</asp:Content>
