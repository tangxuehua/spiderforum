using System.Web;

namespace System.Web.Core
{
    public class Head
    {
        private static readonly string titleKey = "System.Web.Core.Title.Value";
        private static HttpContext context = HttpContext.Current;
        private static Head head = new Head();

        private Head()
        {
            context.Items[titleKey] = string.Empty;
        }

        public static Head Instance
        {
            get
            {
                return head;
            }
        }

        public string Title
        {
            get
            {
                return context.Items[titleKey] as string;
            }
            set
            {
                context.Items[titleKey] = value;
            }
        }
    }
}
