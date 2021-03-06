using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ValuedRadioButton : RadioButton, IValuedControl
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
                    return this.Checked.ToString();
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.Checked = false;
                }
                else if (value.Trim().ToLower() == "false" || value.Trim().ToLower() == "0")
                {
                    this.Checked = false;
                }
                else if (value.Trim().ToLower() == "true" || value.Trim().ToLower() == "1")
                {
                    this.Checked = true;
                }
                else
                {
                    this.Checked = false;
                }
            }
        }
    }

}
