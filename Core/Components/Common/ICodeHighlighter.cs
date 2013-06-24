using System;
using System.Web.UI;

namespace System.Web.Core
{
    public interface ICodeHighlighter
    {
        string Language { get; set; }
        string OriginalText { get; set; }
        Control HighlighterControl { get; }
    }
}
