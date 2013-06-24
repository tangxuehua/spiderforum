<%@ Control Language="C#" Inherits="System.Web.Core.ValuedEditor, System.Web.Core" AutoEventWireup="true" %>
<%@ Import Namespace="System.Web.Core" %>

<script type="text/javascript" src="<%=Globals.ApplicationPath + "/utility/tinymce_3.4.6/jscripts/tiny_mce/tiny_mce.js" %>"></script>

<script type="text/javascript">
    tinyMCE.init({
        mode : "textareas",
        theme : "advanced",
        width:"100%",
        height:"500",
        plugins : "syntaxhl,autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave",
        theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft,syntaxhl",
        theme_advanced_toolbar_location : "top",
        theme_advanced_toolbar_align : "left",
        theme_advanced_statusbar_location : "bottom",
        theme_advanced_resizing : true
    });
</script>

    <script runat="server" type="text/C#">
        private string HTMLDecode(string sourceString)
        {
            if (string.IsNullOrEmpty(sourceString))
            {
                return string.Empty;
            }

            return sourceString.Replace("&amp;", @"&").Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "'");
        }
    </script>

<ctrl:HtmlEditor runat="server" style="display:none;" id="editor" />
<textarea id="bodyArea" name="bodyArea" rows="0" cols="0" style="width:100%;height:60%;border:solid 0;resize:none;"><%=HTMLDecode(editor.Text)%></textarea>