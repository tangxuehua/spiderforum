using System;
using System.Web.Core;

namespace Forum.Business
{
    public class ThreadReleaseStatusValidator : IDataValidator
    {
        #region IDataValidator 成员

        public bool Validate(object parameter)
        {
            try
            {
                int status = (int)parameter;
                return (status == (int)ThreadReleaseStatus.Close || status == (int)ThreadReleaseStatus.Deleted || status == (int)ThreadReleaseStatus.Open);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
