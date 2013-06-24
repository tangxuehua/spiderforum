using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace System.Web.Core
{
    public class Table
    {
        #region Consts

        private const string createEntitySQL = @"INSERT INTO [{0}]({1}) VALUES ({2});SET @EntityId = SCOPE_IDENTITY();";
        private const string updateEntitySQL = @"UPDATE [{0}] SET {1} WHERE EntityId = @EntityId;";
        private const string deleteEntitySQL = @"DELETE FROM [{0}] WHERE EntityId = @EntityId;";
        private const string getEntitySQL = @"SELECT * FROM [{0}] WHERE EntityId = @EntityId;";

        #endregion

        #region Private Members

        private string name = null;
        private List<TableField> fields = new List<TableField>();

        #endregion

        #region Constructors

        public Table(XmlNode xmlNode)
        {
            name = xmlNode.Attributes["name"].Value;
            foreach (XmlNode fieldNode in xmlNode.ChildNodes)
            {
                fields.Add(new TableField(fieldNode));
            }
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
        public List<TableField> Fields
        {
            get
            {
                return fields;
            }
        }
        public string CreateCommandText
        {
            get
            {
                List<string> fieldNames = new List<string>();
                List<string> parameterNames = new List<string>();
                foreach (TableField field in Fields)
                {
                    if (field.Name != "EntityId")
                    {
                        fieldNames.Add(field.Name);
                        parameterNames.Add("@" + field.Name);
                    }
                }
                return string.Format(createEntitySQL, Name, string.Join(",", fieldNames.ToArray()), string.Join(",", parameterNames.ToArray()));
            }
        }
        public string UpdateCommandText
        {
            get
            {
                List<string> setFieldItems = new List<string>();
                string setFieldFormat = "{0} = {1}";
                foreach (TableField field in Fields)
                {
                    if (field.Name != "EntityId")
                    {
                        setFieldItems.Add(string.Format(setFieldFormat, field.Name, "@" + field.Name));
                    }
                }
                return string.Format(updateEntitySQL, Name, string.Join(",", setFieldItems.ToArray()));
            }
        }
        public string DeleteCommandText
        {
            get
            {
                return string.Format(deleteEntitySQL, Name);
            }
        }
        public string RetrieveCommandText
        {
            get
            {
                return string.Format(getEntitySQL, Name);
            }
        }

        #endregion
    }
}
