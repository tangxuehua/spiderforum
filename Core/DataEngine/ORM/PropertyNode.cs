using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Web.Core
{
    public class PropertyNode
    {
        private string propertyPath = null;
        private string propertyName = null;
        private string fieldName = null;
        private PropertyNode parentNode = null;
        private List<PropertyNode> childNodes = new List<PropertyNode>();

        public string PropertyPath
        {
            get
            {
                return propertyPath;
            }
            set
            {
                propertyPath = value;
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
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;
            }
        }
        public PropertyNode ParentNode
        {
            get
            {
                return parentNode;
            }
            set
            {
                parentNode = value;
            }
        }
        public List<PropertyNode> ChildNodes
        {
            get
            {
                return childNodes;
            }
            set
            {
                childNodes = value;
            }
        }
    }
}
