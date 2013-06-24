using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ValuedHiddenField : HiddenField, IValuedControl
    {
        public override string Value
        {
            get 
            {
                if (Page.IsPostBack || AjaxManager.IsCallBack)
                {
                    return Globals.GetControlValue(this);
                }
                else
                {
                    return base.Value;
                }
            }
            set
            {
                base.Value = value;
            }
        }
    }
}
