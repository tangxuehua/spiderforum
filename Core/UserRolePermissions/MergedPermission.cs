namespace System.Web.Core
{
    public class MergedPermission
    {
        #region Private Members

        private long allowMask;
        private long denyMask;

        #endregion

        #region Constructors

        public MergedPermission()
        {
            allowMask = 0;
            denyMask = (long)-1;
        }
        public MergedPermission(long allowMask, long denyMask)
        {
            this.allowMask = allowMask;
            this.denyMask = denyMask;
        }

        #endregion

        #region Public Properties

        public long AllowMask
        {
            get { return allowMask; }
            set { allowMask = value; }
        }
        public long DenyMask
        {
            get { return denyMask; }
            set { denyMask = value; }
        }

        #endregion

        #region Public Methods

        public bool ValidatePermission(long requiredPermission)
        {
            bool bReturn = false;

            if ((denyMask & requiredPermission) == requiredPermission)
                bReturn = false;

            if ((allowMask & requiredPermission) == requiredPermission)
                bReturn = true;

            return bReturn;
        }
        public void MergeRolePermission(Permission rolePermission)
        {
            this.allowMask |= rolePermission.AllowMask.Value;
            this.denyMask |= rolePermission.DenyMask.Value;
        }

        #endregion
    }
}