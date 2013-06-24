using System;

namespace System.Web.Core
{
    public class NullableDateTimeValidator : IDataValidator
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
                DateTime? dateTime = Globals.ConvertType<DateTime?>(parameter);
                return dateTime.HasValue && dateTime >= DateTime.Parse("1753-01-01");
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
