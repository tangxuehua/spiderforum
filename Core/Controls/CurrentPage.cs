using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core 
{
    public class CurrentPage : Label 
    {
        protected override void Render(HtmlTextWriter writer) {

            Control skin = (Control) this.Parent;
            Panel displayPager = (Panel) skin.FindControl("DisplayPager");

            if (TotalPages <= 1)
                return;

            if (displayPager != null)
                displayPager.Visible = true;

            this.Text = String.Format(TextFormat, PageIndex, TotalPages.ToString("n0"), TotalRecords.ToString("n0") );

            base.Render(writer);

        }

        public string TextFormat {
            get {
                Object state = ViewState[ "TextFormat" ];
                if ( state != null ) {
                    return (String)state;
                }
                return ResourceManager.GetString("Utility_CurrentPage_text");
            }
            set {
                ViewState[ "TextFormat" ] = value;
            }
        }

        public int PageIndex {
            get {
                int pageIndex = Convert.ToInt32(ViewState["PageIndex"]);

                if (pageIndex == 0)
                    return 1;

                return pageIndex;
            }
            set {
                ViewState["PageIndex"] = value;
            }
        }

        public int TotalPages {
            get {
                int totalPages = Convert.ToInt32(ViewState["TotalPages"]);

                if (totalPages == 0)
                    return 1;

                return totalPages;
            }
            set {
                ViewState["TotalPages"] = value;
            }
        }

        public int TotalRecords {
            get {
                return Convert.ToInt32(ViewState["TotalRecords"]);
            }
            set {
                ViewState["TotalRecords"] = value;
            }
        }

    }
}
