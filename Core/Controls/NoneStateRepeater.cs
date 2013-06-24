using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class NoneStateRepeater : Repeater
    {
        public NoneStateRepeater()
        {
            EnableViewState = false;
        }
    }
}
