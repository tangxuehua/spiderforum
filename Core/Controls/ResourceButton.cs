using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ResourceButton : Button, IButton
    {
        [Bindable(true),Category( "Appearance" ),Description( "Gets or sets the name of the resource to display." ),DefaultValue( "" ),]
        public virtual String ResourceName 
        {
            get 
            {
                Object state = ViewState["ResourceName"];
                if ( state != null ) 
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

        [Bindable(true),Category( "Appearance" ),Description( "Gets or sets the name of the resource file." ),DefaultValue( null ),]
        public virtual String ResourceFile 
        {
            get 
            {
                Object state = ViewState["ResourceFile"];
                if ( state != null ) 
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

        public Control Control
        {
            get { return this; }
        }

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
            if (!string.IsNullOrEmpty(ResourceName))
            {
                this.Text = ResourceManager.GetString(ResourceName, ResourceFile);
            }
        }

    }
}