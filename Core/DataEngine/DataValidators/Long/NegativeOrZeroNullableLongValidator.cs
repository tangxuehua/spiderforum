using System;

namespace System.Web.Core
{
    public class NegativeOrZeroNullableLongValidator : IDataValidator
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
                long? value = Globals.ConvertType<long?>(parameter);
                return !value.HasValue || value.Value <= 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
