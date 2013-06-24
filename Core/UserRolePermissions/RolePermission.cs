using System;

namespace System.Web.Core
{
    public class RolePermission : Entity
    {
        private Role role = new Role();
        private Permission permission = new Permission();

        public Role Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }
        public Permission Permission
        {
            get
            {
                return permission;
            }
            set
            {
                permission = value;
            }
        }
    }
}