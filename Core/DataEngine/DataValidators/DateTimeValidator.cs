using System;

namespace System.Web.Core
{
    public class DateTimeValidator : IDataValidator
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
                return Globals.ConvertType<DateTime>(parameter) > DateTime.Parse("1753-01-01");
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
