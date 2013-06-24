using System.Web.UI;

namespace System.Web.Core
{
    public interface IText
    {
        bool Visible {get;set;}
        string Text {get;set;}
        
        Control Control { get;}
    }
}