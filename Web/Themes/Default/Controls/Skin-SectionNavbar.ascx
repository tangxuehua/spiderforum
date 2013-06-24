<%@ Control Language="C#" Inherits="Forum.Business.SectionNavbar, Forum.Business"
    AutoEventWireup="true" %>
<%@ Import Namespace="Forum.Business" %>
<div class="wmenu" id="menuBox">
    <ul>
        <ctrl:NoneStateRepeater ID="tabRepeater" runat="server">
            <ItemTemplate>
                <li><a class="<%# GetValue<int>(ForumParameterName.SectionId) == ((Section)Container.DataItem).EntityId.Value ? "cOrange" : "" %>"
                    href="<%# SiteUrls.Instance.GetSectionThreadsUrl(((Section)Container.DataItem).EntityId.Value) %>">
                    <%# ((Section)Container.DataItem).Subject.Value%>
                </a></li>
            </ItemTemplate>
        </ctrl:NoneStateRepeater>
    </ul>
</div>
