<%@ Control Language="C#" Inherits="System.Web.Core.ControlPanelNavbar, System.Web.Core" AutoEventWireup="true" %>
<%@ Import Namespace="System.Web.Core" %>

<script type="text/javascript">
    function toggle(e)
    {
        //��ȡonclick�¼�
        e = e || window.event;
        
        //��ȡtab��titleԪ��
        var title = e.target || e.srcElement; 
        if(title == null) return;  
        
        //��ȡtab��subtabԪ�أ�������չ�����۵���ʽ
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
        
        //�������ͨ���ж�subtab��չ�����۵�������Ӧ��Ҫչ�������۵�
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