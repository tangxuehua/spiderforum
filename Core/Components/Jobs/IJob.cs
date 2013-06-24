using System.Xml;

namespace System.Web.Core
{
    public interface IJob
    {
        void Execute(XmlNode node);
    }
}