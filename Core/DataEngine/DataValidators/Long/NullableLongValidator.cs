using System;

namespace System.Web.Core
{
    public class NullableLongValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            try
            {
                return Globals.ConvertType<long?>(parameter).HasValue;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
