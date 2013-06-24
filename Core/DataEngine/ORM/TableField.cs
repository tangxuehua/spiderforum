using System;
using System.Collections;
using System.Xml;

namespace System.Web.Core
{
    public class TableField
    {
        #region Private Members

        private string name = null;
        private string type = null;
        private string typeEnum = null;
        private string size = null;

        #endregion

        #region Constructors

        public TableField()
        {
        }
        public TableField(XmlNode xmlNode)
        {
            name = xmlNode.Attributes["name"].Value;
            type = xmlNode.Attributes["type"].Value;
            typeEnum = xmlNode.Attributes["typeEnum"].Value;
            size = xmlNode.Attributes["size"].Value;
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public string TypeEnum
        {
            get
            {
                return typeEnum;
            }
            set
            {
                typeEnum = value;
            }
        }
        public string Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        #endregion
    }
}
