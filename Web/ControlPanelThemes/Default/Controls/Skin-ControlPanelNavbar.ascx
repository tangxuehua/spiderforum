<%@ Control Language="C#" Inherits="System.Web.Core.ControlPanelNavbar, System.Web.Core" AutoEventWireup="true" %>
<%@ Import Namespace="System.Web.Core" %>

<script type="text/javascript">
    function toggle(e)
    {
        //获取onclick事件
        e = e || window.event;
        
        //获取tab的title元素
        var title = e.target || e.srcElement; 
        if(title == null) return;  
        
        //获取tab的subtab元素，并设置展开或折叠样式
        var parent = title.parentElement || title.parentNode;
        var subTab = null;
        for(var i = 0; i < parent.childNodes.length; i++)
        {
            if(parent.childNodes[i].tagName == 'UL')
            {
                subTab = parent.childNodes[i];
            }
        }
        if(subTab == null) return;
        
        //下面仅仅通过判断subtab是展开或折叠来决定应该要展开还是折叠
        if(subTab.className.indexOf('show') > 0)
        {
            title.className = title.className.replace('expand','collapse');
            subTab.className = subTab.className.replace('show','hide');
        }
        else if(subTab.className.indexOf('hide') > 0)
        {
            title.className = title.className.replace('collapse','expand');
            subTab.className = subTab.className.replace('hide','show');    
        }
        
    }
</script>
<div class="navbarcontainer">
    <div class="navbar">
        <ctrl:NoneStateRepeater ID="tabRepeater" runat="server">
            <ItemTemplate>
                 <div class="tab"> 
                    <div class="<%# ((Tab)Container.DataItem).IsExpand ? "title expand" : "title collapse" %>" onclick="toggle(event)"> 
                        <%# GetText((Tab)Container.DataItem) %>
                    </div> 
                    <ul class="<%# ((Tab)Container.DataItem).IsExpand ? "subtab show" : "subtab hide" %>">
                        <ctrl:NoneStateRepeater ID="subTabRepeater" runat="server">
                            <ItemTemplate>
                                <li class="<%# ((Tab)Container.DataItem).IsLastChild ? "lastItemContainer" : "itemContainer" %>">
                                    <div class="item">
                                        <a class="normal" target="<%# ((Tab)Container.DataItem).Target %>" href="<%# FormatLink((Tab)Container.DataItem) %>"><%# GetText((Tab)Container.DataItem) %></a>
                                    </div>
                                </li>  
                            </ItemTemplate>
                        </ctrl:NoneStateRepeater>
                    </ul> 
                </div> 
            </ItemTemplate>
        </ctrl:NoneStateRepeater> 
    </div>
</div>