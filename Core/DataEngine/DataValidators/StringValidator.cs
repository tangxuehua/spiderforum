using System;

namespace System.Web.Core
{
    public class StringValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            try
            {
                return !string.IsNullOrEmpty(parameter as string);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
