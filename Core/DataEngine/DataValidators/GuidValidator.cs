using System;

namespace System.Web.Core
{
    public class GuidValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            try
            {
                return ((Guid)parameter) != Guid.Empty;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
