using System;

namespace System.Web.Core
{
    public class NegativeOrZeroNullableIntValidator : IDataValidator
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
                int? value = Globals.ConvertType<int?>(parameter);
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
