﻿using System;

namespace System.Web.Core
{
    public class NullableIntValidator : IDataValidator
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
                return Globals.ConvertType<int?>(parameter).HasValue;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
