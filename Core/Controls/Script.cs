using System;
using System.ComponentModel;
using System.Web.UI;

namespace System.Web.Core
{
    public class Script : Control
    {
        private const string srcFormat = "<script type=\"text/javascript\" charset=\"utf-8\" src=\"{0}\"></script>";

        public virtual string Src
        {
            get
            {
                object state = ViewState["Src"];
                if (state != null)
                {
                    return Globals.ApplicationPath + "/utility/" + state as string;
                }
                return Globals.ApplicationPath + "/utility/global.js";
            }
            set
            {
                ViewState["Src"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(srcFormat, Src);
        }
    }
}