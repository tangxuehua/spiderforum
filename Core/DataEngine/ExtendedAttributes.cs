using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.SqlTypes;

namespace System.Web.Core
{
    [Serializable]
    public class ExtendedAttributes
    {
        private NameValueCollection extendedAttributes = new NameValueCollection();

        public ExtendedAttributes()
        {
        }

        public string GetValue(string name)
        {
            return extendedAttributes[name];
        }

        public void SetValue(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                extendedAttributes.Remove(name);
            }
            else
            {
                extendedAttributes[name] = value;
            }
        }

        public int Count
        {
            get { return extendedAttributes.Count; }
        }

        public SerializerData GetSerializerData()
        {
            SerializerData data = new SerializerData();

            string keys = null;
            string values = null;

            SerializeManager.ConvertFromNameValueCollection(this.extendedAttributes, ref keys, ref values);
            data.Keys = keys;
            data.Values = values;

            return data;
        }

        public void SetSerializerData(SerializerData data)
        {
            this.extendedAttributes = SerializeManager.ConvertToNameValueCollection(data.Keys, data.Values);
        }
    }
}
