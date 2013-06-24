using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace System.Web.Core
{
    public class Configuration
    {
        #region Private Members

        private string userDefaultAvatarPath;
        private string notExistImageDefaultPath;
        private string defaultLanguage;
        private string orMappingFile;
        private string urlsFile;
        private int userOnlineTimeWindow = 20;
        private string registeredUserRoleName = "注册用户";
        private string ownerRoleName = "所有者";
        private string[] registeredDefaultRoles;
        private string[] anonymousDefaultRoles;
        private TEntityList<Role> registeredDefaultRoleList;
        private TEntityList<Role> anonymousDefaultRoleList;
        private Hashtable providers;
        private List<Table> tables = new List<Table>();
        private List<EntityMapping> entityMappings = new List<EntityMapping>();
        private List<Command> commands = new List<Command>();
        private Hashtable sites;
        private Hashtable jobs;
        private Type userType;
        private string siteErrorPage;
        private string notFoundPage;

        private static Configuration instance;

        #endregion

        #region Singleton

        private Configuration()
        {
            Initialize(ConfigurationManager.GetSection("coreConfig") as XmlNode);
        }
        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Configuration();
                }
                return instance;
            }
        }

        #endregion

        #region Public Properties

        public string UserDefaultAvatarPath
        {
            get
            {
                return userDefaultAvatarPath;
            }
        }
        public string NotExistImageDefaultPath
        {
            get
            {
                return notExistImageDefaultPath;
            }
        }
        public string DefaultLanguage
        {
            get
            {
                return defaultLanguage;
            }
        }
        public string UrlsFile
        {
            get
            {
                return urlsFile;
            }
        }
        public string ORMappingFile
        {
            get
            {
                return orMappingFile;
            }
        }
        public int UserOnlineTimeWindow
        {
            get
            {
                return userOnlineTimeWindow;
            }
        }
        public string[] RegisteredDefaultRoles
        {
            get
            {
                return registeredDefaultRoles;
            }
        }
        public string[] AnonymousDefaultRoles
        {
            get
            {
                return anonymousDefaultRoles;
            }
        }
        public TEntityList<Role> RegisteredDefaultRoleList
        {
            get
            {
                if (registeredDefaultRoleList == null)
                {
                    Role role;
                    registeredDefaultRoleList = new TEntityList<Role>();
                    foreach (string roleName in RegisteredDefaultRoles)
                    {
                        role = RoleManager.GetRole(roleName);
                        if (role != null)
                        {
                            registeredDefaultRoleList.Add(role);
                        }
                    }
                }
                return registeredDefaultRoleList;
            }
        }
        public TEntityList<Role> AnonymousDefaultRoleList
        {
            get
            {
                if (anonymousDefaultRoleList == null)
                {
                    Role role;
                    anonymousDefaultRoleList = new TEntityList<Role>();
                    foreach (string roleName in AnonymousDefaultRoles)
                    {
                        role = RoleManager.GetRole(roleName);
                        if (role != null)
                        {
                            anonymousDefaultRoleList.Add(role);
                        }
                    }
                }
                return anonymousDefaultRoleList;
            }
        }
        public string RegisteredUserRoleName
        {
            get
            {
                return registeredUserRoleName;
            }
        }
        public string OwnerRoleName
        {
            get
            {
                return ownerRoleName;
            }
        }
        public Hashtable Providers
        {
            get
            {
                return providers;
            }
        }
        public Hashtable Jobs
        {
            get
            {
                return jobs;
            }
        }
        public Hashtable Sites
        {
            get
            {
                return sites;
            }
        }
        public List<Table> Tables
        {
            get
            {
                return tables;
            }
        }
        public List<EntityMapping> EntityMappings
        {
            get
            {
                return entityMappings;
            }
        }
        public List<Command> Commands
        {
            get
            {
                return commands;
            }
        }
        public Type UserType
        {
            get
            {
                return userType;
            }
        }
        public string SiteErrorPage
        {
            get
            {
                return siteErrorPage;
            }
        }
        public string NotFoundPage
        {
            get
            {
                return notFoundPage;
            }
        }

        #endregion

        #region Public Methods

        public bool IsOwnerRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            return roleName == OwnerRoleName;
        }

        public Command GetCommand(string commandIdent)
        {
            foreach (Command command in commands)
            {
                if (command.CommandIdent == commandIdent)
                {
                    return command;
                }
            }
            return null;
        }

        public string GetDBFieldName(Type entityType, string propertyPath)
        {
            PropertyNode propertyNode = GetPropertyNode(entityType, propertyPath);
            if (propertyNode != null)
            {
                return propertyNode.FieldName;
            }
            return null;
        }
        public PropertyNode GetPropertyNode(Type entityType, string propertyPath)
        {
            PropertyNode propertyNode = null;

            foreach (EntityMapping mapping in entityMappings)
            {
                if (entityType.IsAssignableFrom(mapping.EntityType))
                {
                    propertyNode = mapping.GetPropertyNode(propertyPath);
                    if (propertyNode != null)
                    {
                        return propertyNode;
                    }
                }
            }
            return null;
        }
        public string GetTableName(Type entityType)
        {
            foreach (EntityMapping mapping in entityMappings)
            {
                if (entityType.IsAssignableFrom(mapping.EntityType))
                {
                    return mapping.TableName;
                }
            }
            return null;
        }
        public EntityMapping GetEntityMapping(Type entityType)
        {
            foreach (EntityMapping mapping in entityMappings)
            {
                if (entityType.IsAssignableFrom(mapping.EntityType))
                {
                    return mapping;
                }
            }
            return null;
        }
        public Table GetTable(Type entityType)
        {
            string tableName = GetTableName(entityType);
            foreach (Table table in tables)
            {
                if (table.Name == tableName)
                {
                    return table;
                }
            }
            return null;
        }
        public TableField GetTableField(Type entityType, string propertyPath)
        {
            string fieldName = GetDBFieldName(entityType, propertyPath);
            foreach (TableField tableField in GetTable(entityType).Fields)
            {
                if (tableField.Name == fieldName)
                {
                    return tableField;
                }
            }
            return null;
        }
        public object GetPropertyValue(object targetObject, string propertyName)
        {
            object propertyValue = targetObject.GetType().GetProperty(propertyName).GetValue(targetObject, null);
            if (propertyValue is Property)
            {
                return ((Property)propertyValue).ObjectValue;
            }
            return propertyValue;
        }
        public object GetPropertyPathValue(object targetObject, string propertyPath)
        {
            if (targetObject == null || string.IsNullOrEmpty(propertyPath))
            {
                return null;
            }
            string[] propertyNames = propertyPath.Split(new char[] { '.' });
            object propertyValue = targetObject;
            foreach (string propertyName in propertyNames)
            {
                propertyValue = GetPropertyValue(propertyValue, propertyName);
                if (propertyValue == null)
                {
                    return null;
                }
            }
            return propertyValue;
        }

        #endregion

        #region Private Methods

        private void Initialize(XmlNode configSection)
        {
            if (configSection == null)
            {
                return;
            }
            XmlAttributeCollection attributeCollection = configSection.Attributes;

            XmlAttribute att = attributeCollection["defaultLanguage"];
            if (att != null)
            {
                defaultLanguage = att.Value;
            }
            else
            {
                defaultLanguage = "zh-CN";
            }
            att = attributeCollection["userDefaultAvatarPath"];
            if (att != null)
            {
                userDefaultAvatarPath = att.Value;
            }
            att = attributeCollection["notExistImageDefaultPath"];
            if (att != null)
            {
                notExistImageDefaultPath = att.Value;
            }
            att = attributeCollection["orMappingFile"];
            if (att != null)
            {
                orMappingFile = att.Value;
            }
            else
            {
                orMappingFile = @"ORMappings.xml";
            }
            att = attributeCollection["siteUrlsFile"];
            if (att != null)
            {
                urlsFile = att.Value;
            }
            else
            {
                urlsFile = @"Urls.xml";
            }
            att = attributeCollection["userType"];
            if (att != null)
            {
                userType = Type.GetType(att.Value);
            }
            if (userType == null || !typeof(User).IsAssignableFrom(userType))
            {
                throw new Exception("UserType incorrect.");
            }
            att = attributeCollection["siteErrorPage"];
            if (att != null)
            {
                siteErrorPage = att.Value;
            }
            att = attributeCollection["notFoundPage"];
            if (att != null)
            {
                notFoundPage = att.Value;
            }

            att = attributeCollection["registeredUserRoleName"];
            if (att != null)
            {
                registeredUserRoleName = att.Value;
            }

            XmlAttribute roles = attributeCollection["registeredDefaultRoles"];
            if (roles != null)
            {
                registeredDefaultRoles = roles.Value.Split(';');
            }
            roles = attributeCollection["anonymousDefaultRoles"];
            if (roles != null)
            {
                anonymousDefaultRoles = roles.Value.Split(';');
            }

            try
            {
                if (attributeCollection["userOnlineTimeWindow"] != null &&
                    !string.IsNullOrEmpty(attributeCollection["userOnlineTimeWindow"].Value))
                {
                    userOnlineTimeWindow = int.Parse(attributeCollection["userOnlineTimeWindow"].Value);
                }
            }
            catch
            { }

            GetProviders(configSection);
            GetJobs(configSection);
            GetSites(configSection);

            string file = Globals.MapPath(ORMappingFile);
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            GetTables(doc.DocumentElement);
            GetEntityMappings(doc.DocumentElement);
            GetCommands(doc.DocumentElement);
        }
        private void GetProviders(XmlNode configSection)
        {
            providers = new Hashtable();
            foreach (XmlNode child in configSection.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    foreach (XmlNode provider in child.ChildNodes)
                    {
                        switch (provider.Name)
                        {
                            case "add":
                                providers.Add(provider.Attributes["name"].Value, ProviderFactory.Instance.CreateProvider(provider.Attributes["name"].Value, new ProviderData(provider.Attributes)));
                                break;
                            case "remove":
                                providers.Remove(provider.Attributes["name"].Value);
                                break;
                            case "clear":
                                providers.Clear();
                                break;
                        }
                    }
                    break;
                }
            }
        }
        private void GetTables(XmlNode configSection)
        {
            tables.Clear();
            foreach (XmlNode tableNode in configSection.SelectSingleNode(@"tables").ChildNodes)
            {
                tables.Add(new Table(tableNode));
            }
        }
        private void GetJobs(XmlNode configSection)
        {
            jobs = new Hashtable();
            foreach (XmlNode child in configSection.ChildNodes)
            {
                if (child.Name == "jobs")
                {
                    foreach (XmlNode jobNode in child.ChildNodes)
                    {
                        if (jobNode.NodeType != XmlNodeType.Comment)
                        {
                            XmlAttribute typeAttribute = jobNode.Attributes["type"];
                            XmlAttribute nameAttribute = jobNode.Attributes["name"];

                            Type type = Type.GetType(typeAttribute.Value);
                            if (type != null)
                            {
                                if (!jobs.Contains(nameAttribute.Value))
                                {
                                    Job job = new Job(type, jobNode);
                                    jobs[nameAttribute.Value] = job;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void GetSites(XmlNode configSection)
        {
            sites = new Hashtable();
            foreach (XmlNode child in configSection.ChildNodes)
            {
                if (child.Name == "sites")
                {
                    foreach (XmlNode siteNode in child.ChildNodes)
                    {
                        if (siteNode.NodeType != XmlNodeType.Comment)
                        {
                            XmlAttribute nameAttribute = siteNode.Attributes["name"];
                            XmlAttribute typeAttribute = siteNode.Attributes["type"];
                            XmlAttribute entityProviderNameAttribute = siteNode.Attributes["entityProviderName"];

                            Type type = Type.GetType(typeAttribute.Value);
                            if (type != null)
                            {
                                if (!sites.Contains(nameAttribute.Value))
                                {
                                    ISite site = Activator.CreateInstance(type) as ISite;
                                    if (site == null)
                                    {
                                        throw new Exception(string.Format("The site {0} has the incorrect type {1}.", nameAttribute.Value, typeAttribute.Value));
                                    }
                                    site.EntityProvider = providers[entityProviderNameAttribute.Value] as EntityProvider;
                                    sites[nameAttribute.Value] = site;
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("The site {0} has the incorrect type {1}.", nameAttribute.Value, typeAttribute.Value));
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void GetEntityMappings(XmlNode configSection)
        {
            entityMappings.Clear();
            foreach (XmlNode entityMappingNode in configSection.SelectSingleNode(@"entityMappings").ChildNodes)
            {
                entityMappings.Add(new EntityMapping(entityMappingNode));
            }
        }
        private void GetCommands(XmlNode configSection)
        {
            commands.Clear();
            foreach (XmlNode commandNode in configSection.SelectSingleNode(@"commands").ChildNodes)
            {
                commands.Add(new Command(commandNode));
            }
        }

        #endregion
    }
}