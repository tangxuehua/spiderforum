using System;

namespace System.Web.Core
{
    public class PositiveLongValidator : IDataValidator
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
                return Globals.ConvertType<long>(parameter) > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
