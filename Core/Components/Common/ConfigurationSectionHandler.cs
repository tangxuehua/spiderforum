using System.Configuration;
using System.Xml;

namespace System.Web.Core
{
    public class ConfigurationCoreSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
    public class ConfigurationSiteSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
}