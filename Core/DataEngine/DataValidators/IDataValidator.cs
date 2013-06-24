using System;

namespace System.Web.Core
{
    public interface IDataValidator
    {
        bool Validate(object parameter);
    }
}
