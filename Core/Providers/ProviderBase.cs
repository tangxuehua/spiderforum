using System.Collections.Specialized;

namespace System.Web.Core
{
    /// <summary>
    /// �ṩ����Կ���չ���ṩ����ģ�͵Ļ�ʵ�֡�
    /// </summary>
    public abstract class ProviderBase
    {
        private string name;
        private string description;

        public string Name
        {
            get 
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public virtual void Initialize(string name, NameValueCollection config)
        {
            this.name = name;
            this.description = config["description"];
        }
    }
}