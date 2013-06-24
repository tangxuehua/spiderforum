using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ResourceLink : LiteralControl
    {
        public string NavigateUrl
        {
            get
            {
                Object state = ViewState["NavigateUrl"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["NavigateUrl"] = value;
            }
        }
        public string UrlName
        {
            get
            {
                Object state = ViewState["UrlName"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["UrlName"] = value;
            }
        }
        public string Parameters
        {
            get
            {
                Object state = ViewState["Parameters"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["Parameters"] = value;
            }
        }
        private string[] ParameterCollection
        {
            get
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    return Parameters.Split(new char[] { ","[0], ";"[0], "|"[0] }, StringSplitOptions.None);
                }
                return null;
            }
        }
        public string ResourceName
        {
            get
            {
                Object state = ViewState["ResourceName"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["ResourceName"] = value;
            }
        }
        public string ResourceFile
        {
            get
            {
                Object state = ViewState["ResourceFile"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["ResourceFile"] = value;
            }
        }
        public string Target
        {
            get
            {
                Object state = ViewState["Target"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["Target"] = value;
            }
        }
        public string CssClass
        {
            get
            {
                Object state = ViewState["CssClass"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["CssClass"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            HyperLink link = new HyperLink();

            //set text
            if (!string.IsNullOrEmpty(ResourceName))
            {
                if (!string.IsNullOrEmpty(ResourceFile))
                {
                    link.Text = ResourceManager.GetString(ResourceName, ResourceFile);
                }
                else
                {
                    link.Text = ResourceManager.GetString(ResourceName);
                }
            }
            else
            {
                link.Text = Text;
            }

            //set navigate url
            if (!string.IsNullOrEmpty(UrlName))
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    link.NavigateUrl = UrlManager.Instance.FormatUrl(UrlName, ParameterCollection);
                }
                else
                {
                    link.NavigateUrl = UrlManager.Instance.FormatUrl(UrlName);
                }
            }
            else
            {
                link.NavigateUrl = NavigateUrl;
            }

            //set target
            if (!string.IsNullOrEmpty(Target))
            {
                link.Target = Target;
            }

            //set CssClass
            if (!string.IsNullOrEmpty(CssClass))
            {
                link.CssClass = CssClass;
            }

            //render
            writer.Write(Globals.RenderControl(link));
        }

    }
}
