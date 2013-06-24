using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ResourceLabel : WebControl
    {
        protected override ControlCollection CreateControlCollection() 
        {
            return new EmptyControlCollection(this);
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

        [Bindable(true),Category( "Appearance" ),Description( "Gets or sets the name of the resource to dispaly." ),DefaultValue( "" ),]
        public virtual String ResourceName {
            get {
                Object state = ViewState["ResourceName"];
                if ( state != null ) {
                    return (String)state;
                }
                return "";
            }
            set {
                ViewState["ResourceName"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write( ResourceManager.GetString( ResourceName, ResourceFile ));
        }

    }
}