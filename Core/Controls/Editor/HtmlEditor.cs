using System;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class HtmlEditor : TextBox, ITextEditor
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Initialize();
        }

        public override string Text
        {
            get
            {
                if (string.IsNullOrEmpty(base.Text))
                {
                    if (!string.IsNullOrEmpty(this.Page.Request.Form[this.UniqueID]))
                    {
                        return this.Page.Request.Form[this.UniqueID];
                    }
                    return string.Empty;
                }
                else
                {
                    return base.Text.Trim();
                }
            }
            set
            {
                base.Text = value;
            }
        }

        private void Initialize()
        {
            if (this.Height != Unit.Empty)
            {
                this.Style["height"] = this.Height.ToString();
            }
            else
            {
                this.Style["height"] = "250px;";
            }
            if (this.Width != Unit.Empty)
            {
                this.Style["width"] = this.Width.ToString();
            }
            else
            {
                this.Style["width"] = "100%";
            }
            this.TextMode = TextBoxMode.MultiLine;
        }
    }
}
