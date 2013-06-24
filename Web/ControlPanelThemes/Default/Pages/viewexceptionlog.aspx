<%@ Page Language="C#" Inherits="Forum.Business.ViewExceptionLogPage, Forum.Business" AutoEventWireup="true" MasterPageFile="~/ControlPanelThemes/Default/Masters/ControlPanelContentMaster.master" %>

<asp:Content ContentPlaceHolderID="bcr" runat="Server">

<div class="ViewExceptionLogForm">
    <fieldset>
        <legend>
            <ctrl:ResourceLiteral ID="ResourceLiteral1" runat="server" ResourceName="System_ControlPanel_ExceptionLog_ViewDetailTitle"
                ResourceFile="ControlPanelResources.xml" />
        </legend>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel1" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_Message" />
            <span class="StaticText"><%= CurrentExceptionLog.Message.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel2" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_DateCreated" />
            <span class="StaticText"><%= CurrentExceptionLog.DateCreated.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel3" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_DateLastOccurred" />
            <span class="StaticText"><%= CurrentExceptionLog.DateLastOccurred.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel4" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_PathAndQuery" />
            <span class="StaticText"><%= CurrentExceptionLog.PathAndQuery.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel5" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_HttpVerb" />
            <span class="StaticText"><%= CurrentExceptionLog.HttpVerb.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel6" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_HttpReferrer" />
            <span class="StaticText"><%= CurrentExceptionLog.HttpReferrer.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel7" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_UserAgent" />
            <span class="StaticText"><%= CurrentExceptionLog.UserAgent.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel8" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_IPAddress" />
            <span class="StaticText"><%= CurrentExceptionLog.IPAddress.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel9" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_Frequency" />
            <span class="StaticText"><%= CurrentExceptionLog.Frequency.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel10" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_UserName" />
            <span class="StaticText"><%= CurrentExceptionLog.UserName.Value %></span>
        </div>
        <div class="FormRow">
            <ctrl:ResourceLabel ID="ResourceLabel11" CssClass="FieldName" runat="Server" ResourceFile="ControlPanelResources.xml"
                ResourceName="System_ControlPanel_ExceptionLog_ExceptionDetails" />
            <span class="StaticText"><%= CurrentExceptionLog.ExceptionDetails.Value %></span>
        </div>
        <div class="FormRow SubmitButtonRow">
            <ctrl:ResourceButton ID="ResourceButton1" OnClientClick="window.opener=null;window.close();" ResourceName="Close"
                CssClass="Button" runat="server"></ctrl:ResourceButton>
        </div>
    </fieldset>
</div>

</asp:Content>