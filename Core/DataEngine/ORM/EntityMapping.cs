using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace System.Web.Core
{
    public class EntityMapping
    {
        #region Private Members

        private Type entityType = null;
        private string tableName = null;
        private List<PropertyNode> propertyNodes = new List<PropertyNode>();
        private List<PropertyNode> leafPropertyNodes = new List<PropertyNode>();

        #endregion

        #region Constructors

        public EntityMapping(XmlNode xmlNode)
        {
            entityType = Type.GetType(xmlNode.Attributes["entityType"].Value);
            tableName = xmlNode.Attributes["tableName"].Value;
            foreach (XmlNode propertyXmlNode in xmlNode.ChildNodes)
            {
                ProcessPropertyNode(propertyXmlNode, null, PropertyNodes);
            }
        }

        #endregion

        #region Public Properties

        public Type EntityType
        {
            get
            {
                return entityType;
            }
            set
            {
                entityType = value;
            }
        }
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
        public List<PropertyNode> PropertyNodes
        {
            get
            {
                return propertyNodes;
            }
            set
            {
                propertyNodes = value;
            }
        }
        public List<PropertyNode> LeafPropertyNodes
        {
            get
            {
                return leafPropertyNodes;
            }
            set
            {
                leafPropertyNodes = value;
            }
        }

        #endregion

        #region Public Methods

        public ReturnEntity GetDefaultReturnEntity()
        {
            ReturnEntity returnEntity = new ReturnEntity();

            returnEntity.EntityType = this.EntityType;
            returnEntity.EntityReturnMode = EntityReturnMode.Multiple;
            returnEntity.PropertyNodes.AddRange(this.PropertyNodes);

            return returnEntity;
        }
        public PropertyNode GetPropertyNode(string propertyPath)
        {
            PropertyNode returnNode = null;
            foreach (PropertyNode propertyNode in PropertyNodes)
            {
                returnNode = GetPropertyNode(propertyNode, propertyPath);
                if (returnNode != null)
                {
                    break;
                }
            }
            return returnNode;
        }

        #endregion

        #region Private Methods

        private void ProcessPropertyNode(XmlNode currentXmlNode, PropertyNode parentNode, List<PropertyNode> propertyNodes)
        {
            PropertyNode currentPropertyNode = new PropertyNode();

            currentPropertyNode.PropertyName = currentXmlNode.Attributes["propertyName"].Value;
            if (parentNode == null)
            {
                currentPropertyNode.ParentNode = null;
                currentPropertyNode.PropertyPath = currentPropertyNode.PropertyName;
            }
            else
            {
                currentPropertyNode.ParentNode = parentNode;
                currentPropertyNode.PropertyPath = parentNode.PropertyPath + "." + currentPropertyNode.PropertyName;
            }

            if (currentXmlNode.ChildNodes.Count > 0)
            {
                propertyNodes.Add(currentPropertyNode);
                foreach (XmlNode childNode in currentXmlNode.ChildNodes)
                {
                    ProcessPropertyNode(childNode, currentPropertyNode, currentPropertyNode.ChildNodes);
                }
            }
            else
            {
                currentPropertyNode.FieldName = currentXmlNode.Attributes["fieldName"].Value;
                propertyNodes.Add(currentPropertyNode);
                leafPropertyNodes.Add(currentPropertyNode);
            }
        }
        private PropertyNode GetPropertyNode(PropertyNode propertyNode, string propertyPath)
        {
            if (propertyNode.PropertyPath == propertyPath)
            {
                return propertyNode;
            }

            PropertyNode returnNode = null;
            foreach (PropertyNode childPropertyNode in propertyNode.ChildNodes)
            {
                returnNode = GetPropertyNode(childPropertyNode, propertyPath);
                if (returnNode != null)
                {
                    return returnNode;
                }
            }
            return null;
        }

        #endregion
    }
}
