using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ValuedRadioButtonList : RadioButtonList, IValuedControl
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
                    return SelectedValue;
                }
            }
            set
            {
                ListItem item = Items.FindByValue(value);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }
    }
}
