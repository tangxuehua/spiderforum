using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace System.Web.Core
{
    public class DataProcesser
    {
        #region Private Members
        
        private static EntityProvider defaultEntityProvider = Configuration.Instance.Providers["EntityProvider"] as EntityProvider;

        #endregion

        #region Public Methods

        public static void SwitchProvider(EntityProvider provider)
        {
            defaultEntityProvider = provider;
        }
        public static void ResetProvider()
        {
            defaultEntityProvider = Configuration.Instance.Providers["EntityProvider"] as EntityProvider;
        }
        public static void ProcessRequest(RequestBinder requestBinder)
        {
            ProcessRequest(requestBinder, null);
        }
        public static void ProcessRequests(List<RequestBinder> requestBinders)
        {
            ProcessRequests(requestBinders, null);
        }
        public static void ProcessRequest(RequestBinder requestBinder, EntityProvider entityProvider)
        {
            if (entityProvider != null)
            {
                entityProvider.ProcessRequest(requestBinder);
                return;
            }
            defaultEntityProvider.ProcessRequest(requestBinder);
        }
        public static void ProcessRequests(List<RequestBinder> requestBinders, EntityProvider entityProvider)
        {
            if (entityProvider != null)
            {
                entityProvider.ProcessRequests(requestBinders);
                return;
            }
            defaultEntityProvider.ProcessRequests(requestBinders);
        }

        #endregion
    }
}