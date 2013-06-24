using System.Collections.Specialized;

namespace System.Web.Core
{
    /// <summary>
    /// 提供了针对可扩展的提供程序模型的基实现。
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