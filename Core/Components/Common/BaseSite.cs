using System;
using System.Reflection;

namespace System.Web.Core
{
    public abstract class BaseSite : ISite
    {
        #region Private Members

        private EntityProvider entityProvider = null;

        #endregion

        #region ISite 成员

        public EntityProvider EntityProvider
        {
            get
            {
                return entityProvider;
            }
            set
            {
                entityProvider = value;
            }
        }
        public void CreateUser(MemberInfo memberInfo)
        {
            User user = CreateUserFromMemberInfo(memberInfo);
            if (user != null)
            {
                DataProcesser.SwitchProvider(EntityProvider);
                try
                {
                    CreateUser(user);
                }
                catch
                {
                }
                finally
                {
                    DataProcesser.ResetProvider();
                }
            }
        }
        public void DeleteUser(Guid memberId)
        {
            try
            {
                DataProcesser.SwitchProvider(EntityProvider);
                User user = GetUserFromMemberId(memberId);
                if (user != null)
                {
                    DeleteUser(user.EntityId.Value);
                }
            }
            catch
            { }
            finally
            {
                DataProcesser.ResetProvider();
            }
        }
        public void UpdateUserProperty(Guid memberId, string propertyName, string propertyValue)
        {
            try
            {
                DataProcesser.SwitchProvider(EntityProvider);
                User user = GetUserFromMemberId(memberId);
                if (user != null)
                {
                    bool propertyExist = false;
                    Property property = null;
                    foreach (PropertyInfo propertyInfo in user.GetType().GetProperties())
                    {
                        if (propertyInfo.Name == propertyName)
                        {
                            property = Activator.CreateInstance(propertyInfo.PropertyType) as Property;
                            property.ObjectValue = propertyValue;
                            propertyInfo.SetValue(user, property, null);
                            propertyExist = true;
                            break;
                        }
                    }
                    if (propertyExist)
                    {
                        UpdateUser(user);
                    }
                }
            }
            catch
            { }
            finally
            {
                DataProcesser.ResetProvider();
            }
        }

        #endregion

        #region 钩子方法

        protected abstract User CreateUserFromMemberInfo(MemberInfo memberInfo);
        protected virtual User GetUserFromMemberId(Guid memberId)
        {
            return UserManager.GetUser(memberId);
        }
        protected virtual void CreateUser(User user)
        {
            Engine.Create(user);
            UserManager.CreateUserDefaultRoles(user.EntityId.Value);
            user.SetRoles(Configuration.Instance.RegisteredDefaultRoleList);
        }
        protected virtual void UpdateUser(User user)
        {
            Engine.Update(user);
        }
        protected virtual void DeleteUser(int userId)
        {
            UserManager.DeleteUser(userId);
        }

        #endregion
    }
}