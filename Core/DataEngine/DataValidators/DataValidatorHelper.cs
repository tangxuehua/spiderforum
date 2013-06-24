using System;

namespace System.Web.Core
{
    public class DataValidatorHelper
    {
        public static IDataValidator GetDefaultValidator(Type valueType)
        {
            if (valueType == typeof(int?))
            {
                return new PositiveNullableIntValidator();
            }
            else if (valueType == typeof(int))
            {
                return new PositiveIntValidator();
            }
            else if (valueType == typeof(double?))
            {
                return new PositiveNullableDoubleValidator();
            }
            else if (valueType == typeof(double))
            {
                return new PositiveDoubleValidator();
            }
            else if (valueType == typeof(string))
            {
                return new StringValidator();
            }
            else if (valueType == typeof(Guid))
            {
                return new GuidValidator();
            }
            else if (valueType == typeof(DateTime?))
            {
                return new NullableDateTimeValidator();
            }
            else if (valueType == typeof(DateTime))
            {
                return new DateTimeValidator();
            }
            return null;
        }
    }
}
