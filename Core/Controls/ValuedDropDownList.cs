using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ValuedDropDownList : DropDownList, IValuedControl
    {
        public string Value
        {
            get 
            {
                if (Page.IsPostBack || AjaxManager.IsCallBack)
                {
                    return Globals.GetControlValue(this);
                }
                else
                {
                    return this.SelectedValue;
                }
            }
            set
            {
                ListItem item = this.Items.FindByValue(value);
                if (item != null)
                {
                    item.Selected = true;
                }
                else
                {
                    this.SelectedIndex = -1;
                }
            }
        }
    }
}
