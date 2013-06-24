using System;
using System.ComponentModel;
using System.Web.UI;

namespace System.Web.Core
{
    public class ResourceLiteral : Control
    {
        [Bindable(true),Category("Appearance"),Description("Gets or sets the name of the resource file."),DefaultValue("Resources.xml"),]
        public virtual String ResourceFile
        {
            get
            {
                Object state = ViewState["ResourceFile"];
                if (state != null)
                {
                    return (String)state;
                }
                return "";
            }
            set
            {
                ViewState["ResourceFile"] = value;
            }
        }

        [Bindable(true),Category("Appearance"),Description("Gets or sets the name of the resource to dispaly."),DefaultValue(""),]
        public virtual String ResourceName
        {
            get
            {
                Object state = ViewState["ResourceName"];
                if (state != null)
                {
                    return (String)state;
                }
                return "";
            }
            set
            {
                ViewState["ResourceName"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(ResourceManager.GetString(this.ResourceName, ResourceFile));
        }

    }
}