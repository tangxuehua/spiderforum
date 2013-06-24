using System;

namespace System.Web.Core
{
    public class PositiveNullableDoubleValidator : IDataValidator
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
                double? value = Globals.ConvertType<double?>(parameter);
                return value.HasValue && value.Value > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
