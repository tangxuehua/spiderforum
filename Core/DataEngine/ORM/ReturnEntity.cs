using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Web.Core
{
    public class ReturnEntity
    {
        #region Private Members

        private Entity entity = null;
        private Type entityType = null;
        private string propertyName = null;
        private EntityReturnMode entityReturnMode = EntityReturnMode.Single;
        private List<PropertyNode> propertyNodes = new List<PropertyNode>();
        private ReturnEntity parentReturnEntity = null;
        private List<ReturnEntity> childReturnEntityList = new List<ReturnEntity>();

        #endregion

        #region Constructors

        public ReturnEntity()
        {
        }
        public ReturnEntity(XmlNode returnEntityNode) :
            this(returnEntityNode, (EntityReturnMode)Enum.Parse(typeof(EntityReturnMode), returnEntityNode.Attributes["entityReturnMode"].Value))
        {
        }
        public ReturnEntity(XmlNode returnEntityNode, EntityReturnMode entityReturnMode)
        {
            this.entityType = Type.GetType(returnEntityNode.Attributes["entityType"].Value);
            this.entityReturnMode = entityReturnMode;

            XmlAttribute propertyNameAttribute = returnEntityNode.Attributes["propertyName"];
            if (propertyNameAttribute != null && !string.IsNullOrEmpty(propertyNameAttribute.Value))
            {
                this.propertyName = propertyNameAttribute.Value;
            }

            foreach (XmlNode propertyXmlNode in returnEntityNode.ChildNodes)
            {
                if (propertyXmlNode.Name == "propertyNode")
                {
                    ProcessPropertyNode(propertyXmlNode, null, PropertyNodes);
                }
            }
        }

        #endregion

        #region Public Properties

        public Entity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
            }
        }
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
        public string PropertyName
        {
            get
            {
                return propertyName;
            }
            set
            {
                propertyName = value;
            }
        }
        public EntityReturnMode EntityReturnMode
        {
            get
            {
                return entityReturnMode;
            }
            set
            {
                entityReturnMode = value;
            }
        }
        public List<PropertyNode> PropertyNodes
        {
            get
            {
                return propertyNodes;
            }
        }
        public ReturnEntity ParentReturnEntity
        {
            get
            {
                return parentReturnEntity;
            }
            set
            {
                parentReturnEntity = value;
            }
        }
        public List<ReturnEntity> ChildReturnEntityList
        {
            get
            {
                return childReturnEntityList;
            }
        }

        #endregion

        #region Public Methods

        public string GetReturnFields()
        {
            List<string> returnFields = new List<string>();
            foreach (PropertyNode propertyNode in PropertyNodes)
            {
                GetReturnFieldFromCurrentPropertyNode(propertyNode, returnFields);
            }
            List<string> formattedFields = new List<string>();
            foreach (string field in returnFields)
            {
                formattedFields.Add("t." + field);
            }
            return string.Join(",", formattedFields.ToArray());
        }
        public string GetOrderFieldName(string propertyPath)
        {
            PropertyNode propertyNode = GetPropertyNode(propertyPath);
            if (propertyNode != null)
            {
                return propertyNode.FieldName;
            }
            return null;
        }
        public void RemoveReturnPropertyPath(string propertyPath)
        {
            PropertyNode propertyNode = GetPropertyNode(propertyPath);
            if (propertyNode != null)
            {
                if (propertyNode.ParentNode != null)
                {
                    propertyNode.ParentNode.ChildNodes.Remove(propertyNode);
                }
                else
                {
                    PropertyNodes.Remove(propertyNode);
                }
            }
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
            }
        }
        private void GetReturnFieldFromCurrentPropertyNode(PropertyNode propertyNode, List<string> returnFields)
        {
            if (propertyNode.ChildNodes.Count > 0)
            {
                foreach (PropertyNode childPropertyNode in propertyNode.ChildNodes)
                {
                    GetReturnFieldFromCurrentPropertyNode(childPropertyNode, returnFields);
                }
            }
            else if (!returnFields.Contains(propertyNode.FieldName))
            {
                returnFields.Add(propertyNode.FieldName);
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
