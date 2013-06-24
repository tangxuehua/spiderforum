using System;

namespace System.Web.Core
{
    public interface ISite
    {
        EntityProvider EntityProvider { get; set; }
        void CreateUser(MemberInfo memberInfo);
        void DeleteUser(Guid memberId);
        void UpdateUserProperty(Guid memberId, string propertyName, string propertyValue);
    }
}
