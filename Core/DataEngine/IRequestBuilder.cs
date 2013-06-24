using System.Collections.Generic;

namespace System.Web.Core
{
    public interface IRequestBuilder
    {
        void GetRequests(List<RequestBinder> requestBinders);
    }
}