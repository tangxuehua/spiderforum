using System;
using System.ComponentModel;
using System.Web.UI;

namespace System.Web.Core
{
    public class Style : Control
    {
        private const string linkFormat = "<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />";

        [DefaultValue("screen")]
        public string Media
        {
            get
            {
                object state = ViewState["Media"];
                if (state != null)
                {
                    return (string)state;
                }
                return "screen";
            }
            set
            {
                ViewState["Media"] = value;
            }
        }

        public virtual string Href
        {
            get
            {
                string state = (string)ViewState["Href"];
                if (state != null)
                {
                    return Globals.ApplicationPath + "/themes/default/styles/" + state;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["Href"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Href))
            {
                writer.Write(linkFormat, Href, Media);
            }
        }
    }
}