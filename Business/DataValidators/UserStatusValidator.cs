using System;
using System.Web.Core;

namespace Forum.Business
{
    public class UserStatusValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            try
            {
                int value = (int)parameter;
                return value == (int)UserStatus.Normal || value == (int)UserStatus.Locked;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
