using System;
using System.ComponentModel;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace System.Web.Core
{
    public class BaseBreadCrumb : Control
    {
        #region Private Members

        private Queue<Control> crumbs = new Queue<Control>();

        #endregion

        #region Protected Methods

        protected HtmlAnchor CreateAnchor(string innerText, string href)
        {
            HtmlAnchor anchor = new HtmlAnchor();
            anchor.InnerHtml = innerText;
            anchor.HRef = href;
            return anchor;
        }
        protected Literal CreateLiteral(string innerText)
        {
            Literal literal = new Literal();
            literal.Text = innerText;
            return literal;
        }
        protected virtual void GetCrumbs(Queue<Control> crumbs)
        {
            
        }
        protected override void CreateChildControls()
        {
            GetCrumbs(crumbs);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("您的位置:&nbsp;&nbsp;");

            Control c = null;

            while (crumbs.Count > 0)
            {
                c = crumbs.Dequeue();
                HtmlAnchor anchor = c as HtmlAnchor;
                Literal literal = c as Literal;

                if (anchor != null)
                {
                    anchor.Title = Page.Server.HtmlDecode(anchor.InnerHtml);
                    if ((crumbs.Count > 0) && (anchor.InnerHtml.Length > 30))
                    {
                        anchor.InnerHtml = anchor.InnerHtml.Substring(0, 30) + "...";
                    }
                    anchor.RenderControl(writer);
                }
                else if (literal != null && !string.IsNullOrEmpty(literal.Text))
                {
                    if ((crumbs.Count > 0) && (literal.Text.Length > 30))
                    {
                        literal.Text = literal.Text.Substring(0, 30) + "...";
                    }
                    literal.RenderControl(writer);
                }

                if (crumbs.Count > 0)
                {
                    writer.Write(" > ");
                }
            }
        }

        #endregion
    }
}