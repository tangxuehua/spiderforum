using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class FileUploader : FileUpload
    {
        public FileUploader()
        {
            this.Attributes.Add("onchange", "CheckImage(this)");
        }
    }
}
