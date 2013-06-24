using System;
using System.Security.Principal;
using System.Web;

namespace System.Web.Core
{
    public class User : Entity, IPrincipal
    {
        #region Private Members

        private IIdentity identity = null;
        private GuidType memberId = new GuidType();
        private StringType avatarFileName = new StringType();
        private ByteType avatarContent = new ByteType();
        private StringType language = new StringType();
        private StringType siteTheme = new StringType();
        private TEntityList<Role> roleList = new TEntityList<Role>();
        private TEntityList<Permission> rolePermissionList = null;

        #endregion

        #region Public Properties

        public GuidType MemberId
        {
            get
            {
                return memberId;
            }
            set
            {
                memberId = value;
            }
        }
        public StringType AvatarFileName
        {
            get
            {
                return avatarFileName;
            }
            set
            {
                avatarFileName = value;
            }
        }
        public ByteType AvatarContent
        {
            get
            {
                return avatarContent;
            }
            set
            {
                avatarContent = value;
            }
        }
        public StringType Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
            }
        }
        public StringType SiteTheme
        {
            get
            {
                return siteTheme;
            }
            set
            {
                siteTheme = value;
            }
        }
        public TEntityList<Role> RoleList
        {
            get
            {
                return roleList;
            }
        }
        public bool IsAnonymous
        {
            get
            {
                return EntityId.Value <= 0;
            }
        }
        public bool IsAdministrator
        {
            get
            {
                return IsInRole("系统管理员");
            }
        }

        #endregion

        #region Public Methods

        public void SetRoles(TEntityList<Role> roles)
        {
            RoleList.Clear();
            RoleList.AddRange(roles);
        }
        public MergedPermission GetPermissions()
        {
            MergedPermission mergedPermission = new MergedPermission();

            if (rolePermissionList == null)
            {
                if (IsAnonymous)
                {
                    rolePermissionList = new TEntityList<Permission>();
                    foreach (Role role in Configuration.Instance.AnonymousDefaultRoleList)
                    {
                        Permission permission = RolePermissionManager.GetRolePermission(role.EntityId.Value);
                        if (permission != null)
                        {
                            rolePermissionList.Add(permission);
                        }
                    }
                }
                else
                {
                    rolePermissionList = RolePermissionManager.GetUserPermissions(EntityId.Value);
                }
            }
            foreach (Permission permission in rolePermissionList)
            {
                mergedPermission.MergeRolePermission(permission);
            }

            return mergedPermission;
        }
        public MergedPermission GetPermissions(Entity entity)
        {
            MergedPermission mergedPermission = GetPermissions();

            if (entity != null && entity.IsOwner(this))
            {
                Permission permission = RolePermissionManager.GetOwnerRolePermission();
                if (permission != null)
                {
                    mergedPermission.MergeRolePermission(permission);
                }
            }

            return mergedPermission;
        }

        #endregion

        #region Implementation of IPrincipal

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
            set
            {
                identity = value;
            }
        }

        public bool IsInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            foreach (Role role in RoleList)
            {
                if (role.Name.Value == roleName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}