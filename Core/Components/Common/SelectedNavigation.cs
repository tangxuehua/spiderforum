using System;
using System.Web.UI;

namespace System.Web.Core
{
    public class SelectedNavigation : Control
    {
        private string selected = string.Empty;

        public string Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Context.Items["SelectedNavigation"] = this.Selected;
            base.OnLoad(e);
        }
    }
}
