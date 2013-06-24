using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public interface ITextEditor
    {
        string Text { get; set; }
        Unit Height { get; set; }
        Unit Width { get; set; }
    }
}