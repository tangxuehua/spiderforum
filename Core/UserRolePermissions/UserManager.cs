using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Principal;
using System.Web;

namespace System.Web.Core
{
    public class UserManager
    {
        #region Public Methods

        public static User GetAnonymousUser()
        {
            User user = Activator.CreateInstance(Configuration.Instance.UserType) as User;
            user.SetRoles(Configuration.Instance.AnonymousDefaultRoleList);
            user.Identity = new GenericIdentity(string.Empty);
            return user;
        }
        public static User GetUser(Guid memberId)
        {
            TRequest<User> request = new TRequest<User>();
            request.Data = Activator.CreateInstance(Configuration.Instance.UserType) as User;
            request.Data.MemberId.Value = memberId;
            User user = Engine.GetSingle<User>(request);
            if (user != null)
            {
                InitializeUser(user);
            }
            return user;
        }
        public static User GetUser(int userId)
        {
            TRequest<User> request = new TRequest<User>();
            request.Data = Activator.CreateInstance(Configuration.Instance.UserType) as User;
            request.EntityId = userId;
            return Engine.Get<User>(request);
        }
        public static Reply DeleteUser(int userId)
        {
            TRequest<User> request = new TRequest<User>();
            request.Data = Activator.CreateInstance(Configuration.Instance.UserType) as User;
            request.EntityId = userId;
            return Engine.Delete(request);
        }

        public static void CreateUserDefaultRoles(int userId)
        {
            UserRole userRole = null;
            foreach (Role role in Configuration.Instance.RegisteredDefaultRoleList)
            {
                userRole = new UserRole();
                userRole.UserId.Value = userId;
                userRole.RoleId.Value = role.EntityId.Value;
                Engine.Create(userRole);
            }
        }
        public static TEntityList<User> GetRoleUsers(int roleId)
        {
            TRequest<UserAndRole> request = new TRequest<UserAndRole>();
            request.Data.Role.EntityId.Value = roleId;                     
            return Engine.GetInnerList<User>(Engine.GetAll<UserAndRole>(request), "User");
        }

        public static void UpdateUserAvatar(User user, HttpPostedFile postedFile)
        {
            if (postedFile != null)
            {
                UpdateUserAvatar(user, postedFile.InputStream);
            }
        }
        public static void UpdateUserAvatar(User user, Stream stream)
        {
            if (user == null || stream == null)
            {
                return;
            }

            user.AvatarFileName.Value = Globals.GetFileNewUniqueName();
            user.AvatarContent.Value = Globals.GetBytesFromStream(Globals.ResizeImage(stream, 180, 180));

            Engine.Update(user);
        }
        public static void DeleteUserAvatar(User user)
        {
            user.AvatarFileName.Value = null;
            user.AvatarContent.Value = null;

            Engine.Update(user);
        }

        #endregion

        #region Private Methods

        private static void InitializeUser(User user)
        {
            if (user != null)
            {
                TEntityList<Role> roles = new TEntityList<Role>();
                foreach (UserAndRole userAndRole in RoleManager.GetUserRoles(user.EntityId.Value))
                {
                    roles.Add(userAndRole.Role);
                }
                user.SetRoles(roles);
                user.Identity = HttpContext.Current.User.Identity;
            }
        }

        #endregion
    }
}