using System.Collections.Generic;

namespace System.Web.Core
{
    public class RoleManager
    {
        public static Role GetRole(string name)
        {
            TRequest<Role> request = new TRequest<Role>();
            request.Data.Name.Value = name;
            TEntityList<Role> roles = Engine.GetAll<Role>(request);
            if (roles.Count > 0)
            {
                return roles[0];
            }
            return null;
        }
        public static TEntityList<UserAndRole> GetUserRoles(int userId)
        {
            TRequest<UserAndRole> request = new TRequest<UserAndRole>();
            request.Data.User.EntityId.Value = userId;
            return Engine.GetAll<UserAndRole>(request);
        }
        public static bool IsUserInRole(int userId, int roleId)
        {
            TRequest<UserRole> request = new TRequest<UserRole>();
            request.Data.UserId.Value = userId;
            request.Data.RoleId.Value = roleId;
            return Engine.GetAll(request).Count > 0;
        }
        public static void RemoveUserFromRole(int userId, int roleId)
        {
            TRequest<UserRole> request = new TRequest<UserRole>();
            request.Data.UserId.Value = userId;
            request.Data.RoleId.Value = roleId;
            Engine.DeleteList(request);
        }
        public static void RemoveUserRoles(int userId)
        {
            TRequest<UserRole> request = new TRequest<UserRole>();
            request.Data.UserId.Value = userId;
            Engine.DeleteList(request);
        }
    }
}