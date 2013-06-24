using System.Web.UI;

namespace System.Web.Core
{
    public class HeadControl : Control
    {
        public string Title
        {
            get
            {
                return Head.Instance.Title;
            }
            set
            {
                Head.Instance.Title = value;
            }
        }
        public string TitleResourceName
        {
            get
            {
                object state = ViewState["TitleResourceName"];
                if (state != null)
                {
                    return (string)state;
                }
                return "";
            }
            set
            {
                ViewState["TitleResourceName"] = value;
                Head.Instance.Title = ResourceManager.GetString(value);
            }
        }
    }
}
