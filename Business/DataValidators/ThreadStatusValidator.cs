using System;
using System.Web.Core;

namespace Forum.Business
{
    public class ThreadStatusValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            try
            {
                int status = (int)parameter;
                return (status == (int)ThreadStatus.Normal || status == (int)ThreadStatus.Recommended || status == (int)ThreadStatus.Stick);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
