using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Collections.Generic;

namespace System.Web.Core
{
    public class UrlManager
    {
        #region Private Members

        private static UrlManager instance;
        private LocationSet locationSet;
        private string locationFilter;
        private ListDictionary transforms;
        private Dictionary<string, DynamicPath> dynamicPaths;
        private NameValueCollection paths;
        private ArrayList navigations;
        private string urlsFile;

        #endregion

        #region Singleton Instance

        private UrlManager(string urlsFile)
        {
            Initialize(urlsFile);
            this.urlsFile = urlsFile;
        }
        public static UrlManager Instance
        {
            get
            {
                if (CacheManager.Get("UrlManager") == null)
                {
                    string file = Globals.MapPath(Configuration.Instance.UrlsFile);
                    instance = new UrlManager(file);
                    CacheManager.Max("UrlManager", instance, new CacheDependency(file));
                }
                return instance;
            }
        }

        #endregion

        #region Public Properties

        public string UrlsFile
        {
            get
            {
                return urlsFile;
            }
        }
        public Dictionary<string, DynamicPath> DynamicPaths
        {
            get { return this.dynamicPaths; }
        }
        public LocationSet Locations
        {
            get { return locationSet; }
        }
        public string LocationFilter
        {
            get
            {
                return locationFilter;
            }
        }
        public NameValueCollection Paths
        {
            get { return this.paths; }
        }
        public ArrayList Navigations
        {
            get
            {
                return navigations;
            }
        }

        #endregion

        #region Public Methods

        public string FormatUrl(string name)
        {
            return FormatUrl(name, null);
        }
        public virtual string FormatUrl(string name, params object[] parameters)
        {
            if (parameters == null)
            {
                return this.Paths[name];
            }
            else
            {
                try
                {
                    return string.Format(Paths[name], parameters);
                }
                catch
                { }
                return string.Empty;
            }
        }
        public ParameterValueType GetParameterValue<ParameterValueType>(object name)
        {
            if (name != null)
            {
                string nameStringValue = name.ToString();
                foreach (string key in HttpContext.Current.Request.QueryString.Keys)
                {
                    if (string.Equals(key, nameStringValue, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string parameterValue = HttpContext.Current.Request.QueryString[key];
                        if (!string.IsNullOrEmpty(parameterValue))
                        {
                            return Globals.ChangeType<ParameterValueType>(parameterValue);
                        }
                    }
                }
            }
            return Globals.ChangeType<ParameterValueType>(null);
        }
        public ArrayList CreateNavigation(XmlNode navigationsNode)
        {
            ArrayList navigationList = new ArrayList();
            if (navigationsNode != null)
            {
                foreach (XmlNode node in navigationsNode.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Comment)
                    {
                        XmlAttribute name = node.Attributes["name"];
                        XmlAttribute resourceUrl = node.Attributes["resourceUrl"];
                        XmlAttribute resourceName = node.Attributes["resourceName"];
                        XmlAttribute navigateUrl = node.Attributes["navigateUrl"];
                        XmlAttribute text = node.Attributes["text"];
                        XmlAttribute targetAttr = node.Attributes["target"];
                        XmlAttribute classAttr = node.Attributes["class"];
                        XmlAttribute roles = node.Attributes["roles"];
                        XmlAttribute navType = node.Attributes["navType"];

                        string rolesValue;
                        if (roles == null)
                        {
                            rolesValue = "所有人";
                        }
                        else
                        {
                            rolesValue = roles.Value;
                        }

                        string urlValue;
                        if (resourceUrl == null)
                        {
                            urlValue = navigateUrl.Value;
                        }
                        else
                        {
                            urlValue = FormatUrl(resourceUrl.Value);
                        }

                        string target = string.Empty;
                        if (targetAttr != null)
                        {
                            target = targetAttr.Value;
                        }

                        string _class = string.Empty;
                        if (classAttr != null)
                        {
                            _class = classAttr.Value;
                        }

                        string navTypeValue = null;
                        if (navType != null)
                        {
                            navTypeValue = navType.Value;
                        }

                        NavLink link = new NavLink(name.Value, (resourceName == null) ? null : resourceName.Value, (text == null) ? null : text.Value, urlValue, target, _class, rolesValue, navTypeValue);
                        navigationList.Add(link);
                    }
                }
            }
            return navigationList;
        }

        #endregion

        #region Private Methods

        private void Initialize(string urlsFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(urlsFile);
            CreateDynamicPaths(doc.SelectSingleNode("root/dynamicPaths"));
            CreateLocationSet(doc.SelectSingleNode("root/locations"));
            CreateTransformers(doc.SelectSingleNode("root/transformers"));
            CreateUrls(doc.SelectSingleNode("root/urls"));
            navigations = CreateNavigation(doc.SelectSingleNode("root/navigations"));
        }
        private void CreateDynamicPaths(XmlNode parentNode)
        {
            dynamicPaths = new Dictionary<string, DynamicPath>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    string key = node.Attributes["key"].Value;
                    string pathFormat = node.Attributes["pathFormat"].Value;
                    string valueProvider = node.Attributes["valueProvider"].Value;

                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(pathFormat) && !string.IsNullOrEmpty(valueProvider) && !dynamicPaths.ContainsKey(key))
                    {
                        IObjectsProvider objectsProvider = Activator.CreateInstance(Type.GetType(valueProvider)) as IObjectsProvider;
                        if (objectsProvider != null)
                        {
                            dynamicPaths[key] = new DynamicPath(pathFormat, objectsProvider);
                        }
                    }
                }
            }
        }
        private void CreateLocationSet(XmlNode urlRootNode)
        {
            locationSet = new LocationSet();
            Location location = null;
            foreach (XmlNode node in urlRootNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    XmlAttribute name = node.Attributes["name"];
                    XmlAttribute path = node.Attributes["path"];
                    XmlAttribute exclude = node.Attributes["exclude"];

                    if (name != null && path != null)
                    {
                        bool isExeclude = (exclude != null && bool.Parse(exclude.Value));
                        location = new Location(Globals.ApplicationPath + "/" + path.Value.TrimStart('/'), isExeclude);
                        locationSet.Add(name.Value, location);
                    }
                }
            }
            locationFilter = locationSet.Filter;
        }
        private void CreateTransformers(XmlNode transformers)
        {
            transforms = new ListDictionary();
            foreach (XmlNode node in transformers.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;

                    if (!string.IsNullOrEmpty(key))
                    {
                        transforms[key] = value;
                    }
                }
            }
        }
        private void CreateUrls(XmlNode urlsNode)
        {
            paths = new NameValueCollection();
            string locationName = null;
            Location location = null;

            foreach (XmlNode locationNode in urlsNode.ChildNodes)
            {
                if (locationNode.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                locationName = locationNode.Attributes["name"].Value;
                location = locationSet.FindLocationByName(locationName);
                foreach (XmlNode urlNode in locationNode.ChildNodes)
                {
                    if (urlNode.NodeType == XmlNodeType.Comment)
                    {
                        continue;
                    }
                    string name = urlNode.Attributes["name"].Value;
                    string path = urlNode.Attributes["path"].Value;

                    foreach (string key in transforms.Keys)
                    {
                        path = path.Replace(key, transforms[key].ToString());
                    }

                    paths.Add(name, location.Path + path);

                    XmlAttribute patternAttr = urlNode.Attributes["pattern"];
                    XmlAttribute dynamicpathAttr = urlNode.Attributes["dynamicpath"];
                    XmlAttribute realpathAttr = urlNode.Attributes["realpath"];

                    if (patternAttr != null && dynamicpathAttr != null && realpathAttr != null)
                    {
                        string pattern = location.Path + patternAttr.Value;
                        string realPath = realpathAttr.Value;
                        foreach (string key in transforms.Keys)
                        {
                            pattern = pattern.Replace(key, transforms[key].ToString());
                            realPath = realPath.Replace(key, transforms[key].ToString());
                        }
                        location.Add(new ReWrittenUrl(locationName + "." + name, pattern, DynamicPaths[dynamicpathAttr.Value], realPath));
                    }
                }
            }
        }

        #endregion
    }
}
