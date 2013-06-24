using System;
using System.Web.Core;

namespace Forum.Business
{
    public class EnableValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            try
            {
                int value = (int)parameter;
                return value == (int)EnableStatus.Enable || value == (int)EnableStatus.Disable;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
