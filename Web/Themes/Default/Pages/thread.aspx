<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="Forum.Business.ThreadPage, Forum.Business" MasterPageFile="~/Themes/Default/Masters/ContentMaster.master" %>
<%@ Register TagPrefix="ctrl" TagName="Editor" Src="~/themes/default/controls/Skin-HtmlEditor.ascx" %>
<%@ Import Namespace="Forum.Business" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">
    <ctrl:HeadControl runat="server" TitleResourceName="PageTitle_ThreadDetail" />

    <script runat="server" type="text/C#">
        private bool CanEditPost(Post post)
        {
            return CurrentUser.EntityId.Value == post.AuthorId.Value && ValidatePermission(PermissionType.EditPost, post);
        }
        private string GetEditMyPostUrl(Post post)
        {
            return SiteUrls.Instance.GetMyPostEditUrl(post.EntityId.Value);
        }
    </script>

    <div class="<%= IsThreadExist ? "noneVisible" : "" %>">
        <div class="no-result">
            <p class="item-not-found">
                您访问的帖子不存在，它可能已经被删除了！</p>
        </div>
    </div>
    <div class="<%= IsThreadExist ? "" : "noneVisible" %>">
        <p class="guide">
            <ctrl:ThreadBreadCrumb runat="server" />
        </p>
        <ctrl:NoneStateRepeater ID="list" runat="server" EnableViewState="false">
            <ItemTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="thead">
                    <tbody>
                        <tr>
                            <td width="135px" bgcolor="#D5EBD5">
                            </td>
                            <td bgcolor="#E6F3E6">
                                <a class='<%# CanSetAsRecommended ?"":"noneVisible" %>'
                                        href="javascript:setAsRecommended()" title="将该帖子设置为推荐帖子"><span>设为推荐</span></a><a class='<%# CanSetAsStick ?"":"noneVisible" %>' href="javascript:setAsStick()"
                                    title="将该帖子设置为置顶帖子"><span>设为置顶</span></a>
                                <p class="pageS" id="totalPostsWrapper">
                                    共<%# ((Thread)Container.DataItem).TotalPosts.Value%>条回复
                                </p>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="floorBox">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="135px" bgcolor="#F4F9F4" class="lCon" valign="top">
                                    <div class="userAvatar">
                                        <ctrl:UserAvatar ID="UserAvatar1" runat="server" Width="100" Height="100" UserId="<%# ((Thread)Container.DataItem).AuthorId.Value %>"
                                            AvatarFileName="<%# ThreadUser.AvatarFileName.Value %>" />
                                        <p>
                                            <%# ((Thread)Container.DataItem).Author.Value%></p>
                                    </div>
                                </td>
                                <td>
                                    <div class="conHeader">
                                        <div class="date">
                                            发表于：<%# ((Thread)Container.DataItem).CreateDate.Value%>
                                        </div>
                                    </div>
                                    <div class="conBox">
                                        <div class="innerConBox">
                                            <%# ((Thread)Container.DataItem).Body.Value %>
                                        </div>
                                    </div>
                                    <p class="blank50">
                                    </p>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </ItemTemplate>
        </ctrl:NoneStateRepeater>
        <div id="postListContainer">
            <ctrl:NoneStateRepeater ID="postList" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <div class="floorBox">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td width="135px" bgcolor="#F4F9F4" class="lCon" valign="top">
                                        <div class="userAvatar">
                                            <ctrl:UserAvatar ID="UserAvatar2" runat="server" Width="100" Height="100" UserId="<%# ((PostAndUser)Container.DataItem).User.EntityId.Value %>"
                                                AvatarFileName="<%# ((PostAndUser)Container.DataItem).User.AvatarFileName.Value %>" />
                                            <div class="userName">
                                                <p>
                                                    <%# ((PostAndUser)Container.DataItem).User.NickName.Value%></p>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="conHeader">
                                            <div class="date">
                                                发表于：<%# ((PostAndUser)Container.DataItem).Post.CreateDate.Value%>
                                                <div class="pageS">
                                                    <b>
                                                        <%# ((PostAndUser)Container.DataItem).Post.PostIndex%>
                                                    </b>楼
                                                </div>
                                                <div class="pageS">
                                                    <a class="<%# CanEditPost(((PostAndUser)Container.DataItem).Post) ? "" : "noneVisible" %>" href="<%#GetEditMyPostUrl(((PostAndUser)Container.DataItem).Post) %>" target="_blank">修改回复</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="conBox">
                                            <div class="innerConBox">
                                                <%# ((PostAndUser)Container.DataItem).Post.Body.Value%>
                                            </div>
                                        </div>
                                        <p class="blank50">
                                        </p>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </ItemTemplate>
            </ctrl:NoneStateRepeater>
        </div>
        <p class="blank6">
        </p>

                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td width="135px" align="right" valign="top">
                                回复：</td>
                            <td>
                                <ctrl:Editor runat="server" ID="postContentEditor" />
                            </td>
                        </tr>
                        <tr>
                            <td width="135px" align="right">
                            </td>
                            <td>
                                <ctrl:ResourceButton ResourceName="SaveReply" ID="SubmitButton" CssClass="NormalButton" OnClientClick="return CreatePost(this)" runat="server" /></td>
                        </tr>
                    </tbody>
                </table>

        <div class="blank15">
        </div>
    </div>

    <script language="javascript" type="text/javascript">

    function setAsStick(){
        var bool = confirm("确认要置顶该帖子吗？");
        if(bool == false){
            return;
        }
        <%= ClientID %>.SetAsStick('<%= ClientID %>', setAsStickCallBack);
    }

    function setAsRecommended(){
        var bool = confirm("确认将该帖子设置为推荐帖吗？");
        if(bool == false){
            return;
        }
        <%= ClientID %>.SetAsRecommended('<%= ClientID %>', setAsRecommendedCallBack);
    }

    function CreatePost(source) {
        source.disabled = true;
        source.value = "提交中...";

        var content = tinyMCE.activeEditor.getContent();
        var editor = document.getElementById("<%= postContentEditor.ClientID %>" + "_editor");
        editor.value = content;
        <%= ClientID %>.CreatePost('<%= ClientID %>', content, CreatePostCallBack);

        return false;
    }

    function CreatePostCallBack(result)
    {
        var submitButton = document.getElementById('<%= SubmitButton.ClientID %>');
        submitButton.disabled = false;
        submitButton.value = "发表回复";
        if(result.error != null && result.error != "")
        {
            alert(result.error);
            return;
        }
        document.getElementById("postListContainer").innerHTML = result.value.PostList;
        document.getElementById("totalPostsWrapper").innerHTML = "共" + result.value.TotalPosts + "楼";
        tinyMCE.activeEditor.setContent("");
        tinyMCE.activeEditor.isNotDirty = 1;
        $("pre").each(function() {SyntaxHighlighter.highlight(SyntaxHighlighter, this);});
        hideToolbars();
    }

    function setAsStickCallBack(result)
    {
        if(result.error != null && result.error != "")
        {
            alert(result.error);
            return false;
        }
        alert("帖子置顶成功！");
    }
    function setAsRecommendedCallBack(result)
    {
        if(result.error != null && result.error != "")
        {
            alert(result.error);
            return false;
        }
        alert("帖子设为推荐成功！");
    }

    SyntaxHighlighter.all();
    addEvent(window, "load", hideToolbars);
    function hideToolbars() {
        $("div.toolbar").css("display","none");
    }

    </script>

</asp:Content>
