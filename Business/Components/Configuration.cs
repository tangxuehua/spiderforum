using System.Configuration;
using System.Xml;

namespace Forum.Business
{
    public class ForumConfiguration
    {
        #region Private Members

        private string forumSectionAdminRoleName;
        private static ForumConfiguration instance;

        #endregion

        #region Singleton

        private ForumConfiguration()
        {
            Initialize(ConfigurationManager.GetSection("siteConfig") as XmlNode);
        }
        public static ForumConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ForumConfiguration();
                }
                return instance;
            }
        }

        #endregion

        #region Public Properties

        public string ForumSectionAdminRoleName
        {
            get
            {
                return forumSectionAdminRoleName;
            }
        }

        #endregion

        #region Helpers

        private void Initialize(XmlNode configSection)
        {
            if (configSection == null)
            {
                return;
            }

            XmlAttributeCollection attributeCollection = configSection.Attributes;

            XmlAttribute att = attributeCollection["forumSectionAdminRoleName"];
            if (att != null)
            {
                forumSectionAdminRoleName = att.Value;
            }
            else
            {
                forumSectionAdminRoleName = "°æÖ÷";
            }
        }

        #endregion
    }
}