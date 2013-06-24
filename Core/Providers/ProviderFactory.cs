using System;

namespace System.Web.Core
{
    /// <summary>
    /// This class provides the method to create a specific provider.
    /// </summary>
    public class ProviderFactory
    {
        #region Private Members

        private static ProviderFactory instance = new ProviderFactory();

        #endregion

        #region Instance

        public static ProviderFactory Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Public Methods

        public ProviderBase CreateProvider(string providerName, ProviderData providerData)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                throw new Exception("Provider name can not be null.");
            }
            ProviderBase instance;
            Type type = Type.GetType(providerData.Type);
            object newObject = null;

            if (type != null)
            {
                newObject = Activator.CreateInstance(type);
            }
            if (newObject == null)
            {
                throw new Exception(string.Format("Can not create provider, provider name: {0}", providerName));
            }

            instance = newObject as ProviderBase;

            if (instance == null)
            {
                throw new Exception(string.Format("Provider type invalid, provider name: {0}", providerName));
            }

            instance.Initialize(providerName, providerData.Attributes);

            return instance;
        }

        #endregion
    }
}