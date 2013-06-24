using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace System.Web.Core
{
    /// <summary>
    /// Provides all the entity operation methods.
    /// </summary>
    public abstract class EntityProvider : ProviderBase
    {
        public abstract void ProcessRequest(RequestBinder requestBinder);
        public abstract void ProcessRequests(List<RequestBinder> requestBinders);
    }
}