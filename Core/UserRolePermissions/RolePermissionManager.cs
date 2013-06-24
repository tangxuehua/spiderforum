namespace System.Web.Core
{
    public class RolePermissionManager
    {
        public static Permission GetOwnerRolePermission()
        {
            Role ownerRole = RoleManager.GetRole(Configuration.Instance.OwnerRoleName);
            if (ownerRole != null)
            {
                TRequest<RolePermission> request = new TRequest<RolePermission>();
                request.Data.Role.EntityId.Value = ownerRole.EntityId.Value;
                TEntityList<RolePermission> rolePermissions = Engine.GetAll<RolePermission>(request);
                if (rolePermissions.Count > 0)
                {
                    return rolePermissions[0].Permission;
                }
            }
            return null;
        }
        public static Permission GetRolePermission(int roleId)
        {
            TRequest<RolePermission> request = new TRequest<RolePermission>();
            request.Data.Role.EntityId.Value = roleId;
            TEntityList<RolePermission> rolePermissions = Engine.GetAll<RolePermission>(request);
            if (rolePermissions.Count > 0)
            {
                return rolePermissions[0].Permission;
            }
            return null;
        }
        public static TEntityList<Permission> GetUserPermissions(int userId)
        {
            TRequest<UserPermission> request = new TRequest<UserPermission>();
            request.Data.User.EntityId.Value = userId;
            TEntityList<UserPermission> userPermissions = Engine.GetAll<UserPermission>(request);
            TEntityList<Permission> permissions = new TEntityList<Permission>();
            foreach (UserPermission userPermission in userPermissions)
            {
                permissions.Add(userPermission.RolePermission.Permission);
            }
            return permissions;
        }
    }
}