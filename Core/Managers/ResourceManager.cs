using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace System.Web.Core
{
    public class ResourceManager
    {
        public static NameValueCollection GetSupportedLanguages()
        {
            string cacheKey = "SupportedLanguages";

            NameValueCollection supportedLanguages = CacheManager.Get(cacheKey) as NameValueCollection;

            if (supportedLanguages == null)
            {
                string filePath = Globals.MapPath("~/Languages/languages.xml");
                CacheDependency dp = new CacheDependency(filePath);
                supportedLanguages = new NameValueCollection();

                XmlDocument d = new XmlDocument();
                d.Load(filePath);

                foreach (XmlNode n in d.SelectSingleNode("root").ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment)
                    {
                        supportedLanguages.Add(n.Attributes["key"].Value, n.Attributes["name"].Value);
                    }
                }

                CacheManager.Max(cacheKey, supportedLanguages, dp);
            }

            return supportedLanguages;
        }
        public static string GetSupportedLanguage(string language)
        {
            return GetSupportedLanguage(language, Configuration.Instance.DefaultLanguage);
        }
        public static string GetSupportedLanguage(string language, string languageDefault)
        {
            NameValueCollection supportedLanguages = GetSupportedLanguages();
            string supportedLanguage = supportedLanguages[language];
            if (!string.IsNullOrEmpty(supportedLanguage))
            {
                return language;
            }
            else
            {
                return languageDefault;
            }
        }

        public static string GetString(string name)
        {
            return GetString(name, "Resources.xml");
        }
        public static string GetString(string name, string fileName)
        {
            Hashtable resources = new Hashtable();

            if (!string.IsNullOrEmpty(fileName))
            {
                resources = GetResources(fileName);
            }
            else
            {
                resources = GetResources("Resources.xml");
            }

            return resources[name] as string;
        }
        private static Hashtable GetResources(string fileName)
        {
            string userLanguage = ((User)HttpContext.Current.User).Language.Value;
            if (string.IsNullOrEmpty(userLanguage))
            {
                userLanguage = "zh-CN";
            }
            string cacheKey = userLanguage + fileName;
            Hashtable resources = CacheManager.Get(cacheKey) as Hashtable;

            if (resources == null)
            {
                resources = new Hashtable();
                XmlDocument d = new XmlDocument();
                string resourceFile = Globals.MapPath(@"~/Languages/" + userLanguage + @"/" + fileName);

                d.Load(resourceFile);
                foreach (XmlNode n in d.SelectSingleNode("root").ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment)
                    {
                        if (resources[n.Attributes["name"].Value] == null)
                        {
                            resources.Add(n.Attributes["name"].Value, n.InnerText);
                        }
                        else
                        {
                            resources[n.Attributes["name"].Value] = n.InnerText;
                        }
                    }
                }

                CacheManager.Max(cacheKey, resources, new CacheDependency(resourceFile));
            }

            return resources;
        }
    }
}
