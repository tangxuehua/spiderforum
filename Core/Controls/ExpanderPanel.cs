using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    /// <summary>
    /// 实现折叠/展开 功能
    /// </summary>
    public class ExpanderPanel : WebControl, INamingContainer
    {
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        #region CompositeControl Pattern

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        #endregion

        #region Properties

        #region Button

        /// <summary>
        /// 展开状态的按钮样式
        /// </summary>
        [
        System.ComponentModel.DefaultValue("expanderPanelButtonOpened"),
        ]
        public virtual String ButtonOpenedCssClass
        {
            get
            {
                Object state = ViewState["ButtonOpenedCssClass"];
                if (state != null)
                    return (String)state;

                return "expanderPanelButtonOpened";
            }
            set { ViewState["ButtonOpenedCssClass"] = value; }
        }


        /// <summary>
        /// 折叠状态的按钮样式
        /// </summary>
        [
        System.ComponentModel.DefaultValue("expanderPanelButtonClosed"),
        ]
        public virtual String ButtonClosedCssClass
        {
            get
            {
                Object state = ViewState["ButtonClosedCssClass"];
                if (state != null)
                    return (String)state;

                return "expanderPanelButtonClosed";
            }
            set { ViewState["ButtonClosedCssClass"] = value; }
        }

        #endregion

        #region HeaderLabel or HeaderLink

        /// <summary>
        /// 折叠控件的标题
        /// Gets or sets the text shown next to the toggle button
        /// </summary>
        [System.ComponentModel.DefaultValue("标题")]
        public virtual String Text
        {
            get
            {
                if (ResourceName != null)
                    return ResourceManager.GetString(ResourceName);

                Object state = ViewState["Text"];
                if (state != null)
                    return (String)state;

                return "标题";
            }
            set { ViewState["Text"] = value; }
        }

        public virtual String ResourceName
        {
            get { return ViewState["ResourceName"] as string; }
            set { ViewState["ResourceName"] = value; }
        }

        /// <summary>
        /// 导航链接
        /// </summary>
        public virtual String NavigateUrl
        {
            get { return ViewState["NavigateUrl"] as string; }
            set { ViewState["NavigateUrl"] = value; }
        }


        /// <summary>
        /// 文本样式
        /// Gets or sets the css class of the text next to the toggle button
        /// </summary>
        [System.ComponentModel.DefaultValue("")]
        public virtual String TextCssClass
        {
            get
            {
                Object state = ViewState["TextCssClass"];
                if (state != null)
                    return (String)state;

                return "";
            }
            set { ViewState["TextCssClass"] = value; }
        }

        #endregion

        /// <summary>
        /// 需要折叠的模块ID
        /// Gets or sets the ID of the control which is the target of the visibility-toggling behavior.
        /// </summary>
        [
        Description("Gets or sets the ID of the control which is the target of the visibility-toggling behavior."),
        Category("Behavior"),
        DefaultValue(""),
        ]
        public virtual String ControlToToggle
        {
            get
            {
                object savedState = this.ViewState["ControlToToggle"];
                if (savedState != null)
                    return (String)savedState;

                return "";
            }
            set
            {
                this.ViewState["ControlToToggle"] = value;
                this.cachedTargetControl = null;
            }
        }


        /// <summary>
        /// 折叠状态
        /// Gets or sets whether the target control is visible or not
        /// </summary>
        [
        System.ComponentModel.DefaultValue(false),
        ]
        public virtual Boolean Collapsed
        {
            get
            {
                Object state = ViewState["Collapsed"];
                if (state != null)
                    return (Boolean)state;

                return false;
            }
            set { ViewState["Collapsed"] = value; }
        }


        /// <summary>
        /// Gets or sets the order of the button and text
        /// </summary>
        [
        System.ComponentModel.DefaultValue(System.Web.UI.WebControls.TextAlign.Left),
        ]
        public virtual System.Web.UI.WebControls.TextAlign TextAlign
        {
            get
            {
                Object state = ViewState["TextAlign"];
                if (state != null)
                    return (System.Web.UI.WebControls.TextAlign)state;

                return System.Web.UI.WebControls.TextAlign.Left;
            }
            set { ViewState["TextAlign"] = value; }
        }


        #endregion

        #region Lifecycle

        /// <exclude />
        protected override void CreateChildControls()
        {
            Controls.Clear();

            tracker = new HtmlInputHidden();
            tracker.Name = "tracker";
            tracker.ID = "tracker";
            Controls.Add(tracker);

            button = new HyperLink();
            button.ID = "clicker";
            this.Controls.Add(button);


            if (string.IsNullOrEmpty(this.NavigateUrl))
            {
                headerLabel = new Label();
                headerLabel.ID = "label";
                headerLabel.Attributes["style"] = "cursor: pointer;";
                this.Controls.Add(headerLabel);
            }
            else
            {
                headerLink = new HyperLink();
                headerLink.ID = "headerLink";
                this.Controls.Add(headerLink);
            }
        }

        /// <exclude />
        protected override void OnPreRender(EventArgs e)
        {
            this.EnsureChildControls();
            base.OnPreRender(e);

            Collapsed = false;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(scriptKey))
            {
                Page.ClientScript.RegisterClientScriptInclude(scriptKey, Page.ResolveUrl("~/Utility/tunynet_web_ui/ExpanderPanelScript.js"));
            }

            this.tracker.Value = this.Collapsed.ToString();

            if (this.Collapsed)
                this.targetStyle["display"] = "none";
            else
            {
                if (this.targetStyle["display"] != null)
                    this.targetStyle.Remove("display");
            }
        }

        /// <exclude />
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);

            ApplyPropertiesToChildren();

            base.Render(writer);
        }

        private void ApplyPropertiesToChildren()
        {
            this.EnsureChildControls();

            this.button.Attributes["onclick"] =
                String.Format(
                    "return ExpanderPanel_Toggle('{0}','{1}','{2}','{3}','{4}');",
                    this.targetControl.ClientID,
                    this.button.ClientID,
                    this.tracker.ClientID,
                    this.ButtonOpenedCssClass,
                    this.ButtonClosedCssClass
                );

            if (string.IsNullOrEmpty(this.NavigateUrl))
            {
                this.headerLabel.Text = this.Text;
                this.headerLabel.Attributes["onclick"] =
                    String.Format(
                        "return ExpanderPanel_Toggle('{0}','{1}','{2}','{3}','{4}');",
                        this.targetControl.ClientID,
                        this.button.ClientID,
                        this.tracker.ClientID,
                        this.ButtonOpenedCssClass,
                        this.ButtonClosedCssClass
                    );
                this.headerLabel.CssClass = this.TextCssClass;
            }
            else
            {
                headerLink.Text = this.Text;
                headerLink.NavigateUrl = this.NavigateUrl;
                headerLink.CssClass = this.TextCssClass;
            }

            if (this.Collapsed)
                this.button.CssClass = this.ButtonOpenedCssClass;
            else
                this.button.CssClass = this.ButtonClosedCssClass;
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Target Control

        private Control targetControl
        {
            get
            {
                if (cachedTargetControl == null)
                    this.cachedTargetControl = this.NamingContainer.FindControl(this.ControlToToggle);

                return this.cachedTargetControl;
            }
        }
        private Control cachedTargetControl = null;

        private System.Web.UI.CssStyleCollection targetStyle
        {
            get
            {
                WebControl targetWebControl = this.targetControl as WebControl;
                if (targetWebControl != null)
                    return targetWebControl.Style;

                System.Web.UI.HtmlControls.HtmlControl targetHtmlControl = this.targetControl as System.Web.UI.HtmlControls.HtmlControl;
                if (targetHtmlControl != null)
                    return targetHtmlControl.Style;

                return null;
            }
        }

        #endregion

        private static readonly String scriptKey = "SpaceBuilder.Controls.Utils.ExpanderPanel";
        private HtmlInputHidden tracker;
        private Label headerLabel;
        private HyperLink headerLink;
        private HyperLink button;

    }
}
