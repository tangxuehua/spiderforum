using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ValuedTextBox : TextBox, IValuedControl
    {
        public ValuedTextBox()
        {
            this.MaxLength = 50;
            this.AutoCompleteType = AutoCompleteType.Disabled;
        }
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
                    return Text;
                }
            }
            set
            {
                this.Text = value;
            }
        }
    }

}
