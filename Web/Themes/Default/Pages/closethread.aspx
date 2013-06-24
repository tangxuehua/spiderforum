<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="Forum.Business.CloseThreadPage, Forum.Business" MasterPageFile="~/Themes/Default/Masters/ContentMaster.master" %>
<%@ Import Namespace="Forum.Business" %>
<%@ Import Namespace="System.Web.Core" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">
    <ctrl:HeadControl runat="server" TitleResourceName="PageTitle_CloseThread" />

    <script runat="server" type="text/C#">

        private bool IsThreadAuthor(Post post)
        {
            return Thread.AuthorId.Value == post.AuthorId.Value;
        }

    </script>

    <div class="<%= Thread != null ? "noneVisible" : "" %>">
        <div class="no-result">
            <p class="item-not-found">您访问的帖子不存在，它可能已经被管理员删除了！</p>
        </div>
    </div>
    <div class="<%= Thread != null ? "" : "noneVisible" %>">
        <p class="guide"><ctrl:ThreadBreadCrumb ID="ThreadBreadCrumb1" runat="server" /></p>
        <ctrl:NoneStateRepeater ID="list" runat="server" EnableViewState="false">
            <ItemTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="thead">
                    <tbody>
                        <tr>
                            <td width="135" bgcolor="#D5EBD5">
                            </td>
                            <td bgcolor="#E6F3E6">
                                <div class="threadTitle">
                                    <span class="<%# Thread.ThreadReleaseStatus.Value==(int)ThreadReleaseStatus.Close ? "noneVisible" : "" %>"><%# Page.Server.HtmlEncode(((Thread)Container.DataItem).Subject.Value) %></span>
                                    <span class="<%# Thread.ThreadReleaseStatus.Value==(int)ThreadReleaseStatus.Close ? "red" : "noneVisible" %>">【该帖子已经结帖】</span>
                                </div>
                                <div class="pageS" id="totalPostsWrapper">
                                    共<%# ((Thread)Container.DataItem).TotalPosts.Value%>楼
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="floorBox">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td width="135" bgcolor="#F4F9F4" class="lCon" valign="top">
                                    <div class="userAvatar">
                                        <ctrl:UserAvatar ID="UserAvatar1" runat="server" Width="100" Height="100" UserId="<%# ((Thread)Container.DataItem).AuthorId.Value %>" AvatarFileName="<%# ThreadUser.AvatarFileName.Value %>" />
                                        <p><%# ((Thread)Container.DataItem).Author.Value%>(<%# ThreadUser.TotalMarks.Value%>)</p>
                                    </div>
                                </td>
                                <td>
                                    <div class="conHeader">
                                        <div class="date">
                                            发表于：<%# ((Thread)Container.DataItem).CreateDate.Value%>
                                            <div class="pageS">
                                                问题点数：<var id="point" style="background: yellow; border: solid 1px red"><%# ((Thread)Container.DataItem).ThreadMarks.Value%></var>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="conBox">
                                        <%# ((Thread)Container.DataItem).Body.Value %>
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
                                    <td width="135" bgcolor="#F4F9F4" class="lCon" valign="top">
                                        <div class="userAvatar">
                                            <ctrl:UserAvatar ID="UserAvatar2" runat="server" Width="100" Height="100" UserId="<%# ((PostAndUser)Container.DataItem).User.EntityId.Value %>" AvatarFileName="<%# ((PostAndUser)Container.DataItem).User.AvatarFileName.Value %>" />
                                            <div class="userName">
                                                <p><%# ((PostAndUser)Container.DataItem).User.NickName.Value%>(<%# ((PostAndUser)Container.DataItem).User.TotalMarks.Value%>)</p>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="conHeader">
                                            <div class="date">
                                                发表于：<%# ((PostAndUser)Container.DataItem).Post.CreateDate.Value%>
                                                <div class="pageS">
                                                    <b><%# ((PostAndUser)Container.DataItem).Post.PostIndex%></b>楼&nbsp;
                                                    <span class="<%# Thread.ThreadReleaseStatus.Value==(int)ThreadReleaseStatus.Close ? "noneVisible" : "" %>">得分:<%# IsThreadAuthor(((PostAndUser)Container.DataItem).Post) ? "[不能给自己分]" : "" %></span>
                                                    <input type="hidden" name="tb_isCurrentUser" value='<%# IsThreadAuthor(((PostAndUser)Container.DataItem).Post) ? 1 : 0 %>' />
                                                    <input type="hidden" name="tb_user" value='<%# ((PostAndUser)Container.DataItem).Post.AuthorId.Value %>' />
                                                    <input maxlength="3" class="<%# IsThreadAuthor(((PostAndUser)Container.DataItem).Post) || Thread.ThreadReleaseStatus.Value==(int)ThreadReleaseStatus.Close ? "noneVisible" : "" %>" type="text" name="tb_score" value='0' />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="conBox">
                                            <%# ((PostAndUser)Container.DataItem).Post.Body.Value %>
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
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tfoot">
            <tbody>
                <tr>
                    <td width="135" bgcolor="#D5EBD5">
                        <td bgcolor="#E6F3E6">
                            <div style="float: right; padding-right: 6px; margin-left: 8px;">
                            </div>
                        </td>
                </tr>
            </tbody>
        </table>

        <div class="blank10"></div>
        <div>
            <input type="button" id="submitButton" onclick="closeThread()" class="NormalButton" value="确认结贴" />
        </div>

        <div class="blank15"></div>

        <script type="text/javascript">
            document.getElementById('submitButton').disabled="<%= Thread == null || Thread.ThreadReleaseStatus.Value == (int)ThreadReleaseStatus.Close ? "disabled" : "" %>";
        </script>
    </div>

    <script type="text/javascript">

    /*<![CDATA[*/

    function closeThread(){
        var threadId = <%= UrlManager.Instance.GetParameterValue<int>(ForumParameterName.ThreadId) %>;
        var points = document.getElementById("point").innerHTML;
        var isCurrentUsers = document.getElementsByName("tb_isCurrentUser");
        var users = document.getElementsByName("tb_user");
        var txts = document.getElementsByName("tb_score");
      
        var userIds = '';
        var scores = '';
        var hasReply = false;
        
        for(var i = 0;i < txts.length; i++)
        {
            var score = new Number(txts[i].value);
            var isCurrentUser = new Number(isCurrentUsers[i].value);
            if (isCurrentUser == 1)
            {    
                continue;
            }
            else
            {
                hasReply = true;
            }
            if (score < 0)
            {
                alert("不能给负分");
                focusOnPoint(txts[i]);
                return false;
            }
            else if (score > 0)
            {   
                points = points - score;
                   if(userIds == '')
                    userIds = users[i].value;
                else
                    userIds = userIds + ":" + users[i].value;
                    
                if(scores == '')
                    scores = txts[i].value;
                else
                    scores = scores + ":" + txts[i].value;
            }
        }

        if(points != 0 && hasReply){
            alert("给分与总分不符,请重新分配!");
            if(txts.length > 0)
                focusOnPoint(txts[0]);
            return false;
        }
        if(confirm('您确定要结帖吗?'))
        {
            <%= ClientID %>.CloseCurrentThread('<%= ClientID %>', threadId, userIds, scores, showResult);     
        }
        return false;
    }

    function showResult(result)
    {  
        if(result.error != null && result.error != "")
        {
            alert(result.error);
            return;
        }
        alert("结帖成功！");
        window.close();
    }

    SyntaxHighlighter.all();
    addEvent(window, "load", hideToolbars);
    function hideToolbars() {
        $("div.toolbar").css("display","none");
    }

    /*]]>*/

    </script>

</asp:Content>