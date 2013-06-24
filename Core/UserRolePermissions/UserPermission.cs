using System;
using System.Xml.Serialization;

namespace System.Web.Core
{
    public class UserPermission : Entity
    {
        private User user = new User();
        private RolePermission rolePermission = new RolePermission();

        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
        public RolePermission RolePermission
        {
            get
            {
                return rolePermission;
            }
            set
            {
                rolePermission = value;
            }
        }
    }
}