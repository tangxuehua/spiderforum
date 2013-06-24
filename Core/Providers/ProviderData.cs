using System.Collections.Specialized;
using System.Xml;

namespace System.Web.Core
{
    public class ProviderData
    {
        private string name = null;
        private string providerType = null;
        private NameValueCollection providerAttributes = new NameValueCollection();

        public ProviderData(XmlAttributeCollection attributes)
        {
            name = attributes["name"].Value;
            providerType = attributes["type"].Value;
            foreach (XmlAttribute attribute in attributes)
            {
                if ((attribute.Name != "name") && (attribute.Name != "type"))
                {
                    providerAttributes.Add(attribute.Name, attribute.Value);
                }
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Type
        {
            get
            {
                return providerType;
            }
        }
        public NameValueCollection Attributes
        {
            get
            {
                return providerAttributes;
            }
        }
    }
}