using System;
using System.Xml.Serialization;

namespace System.Web.Core
{
    public class Permission : Entity
    {
        #region Private Members

        private IntType roleId = new IntType();
        private LongType allowMask = new LongType();
        private LongType denyMask = new LongType();

        #endregion

        #region Constructors

        public Permission()
        {
            allowMask.Value = 0;
            denyMask.Value = (long)-1;
        }
        public Permission(int roleId, long allowMask, long denyMask)
        {
            this.roleId.Value = roleId;
            this.allowMask.Value = allowMask;
            this.denyMask.Value = denyMask;
        }

        #endregion

        #region Public Properties

        public IntType RoleId
        {
            get
            {
                return roleId;
            }
            set
            {
                roleId = value;
            }
        }
        public LongType AllowMask
        {
            get
            {
                return allowMask;
            }
            set
            {
                allowMask = value;
            }
        }
        public LongType DenyMask
        {
            get
            {
                return denyMask;
            }
            set
            {
                denyMask = value;
            }
        }

        #endregion

        #region Public Methods

        public bool GetBit(long mask)
        {
            bool bReturn = false;

            if ((denyMask.Value & mask) == mask)
                bReturn = false;

            if ((allowMask.Value & mask) == mask)
                bReturn = true;

            return bReturn;
        }
        public void SetBit(long mask, AccessControlEntry accessControl)
        {
            switch (accessControl)
            {
                case AccessControlEntry.Allow:
                    allowMask.Value |= ((long)mask & (long)-1);
                    denyMask.Value &= ~((long)mask & (long)-1);
                    break;
                case AccessControlEntry.NotSet:
                    allowMask.Value &= ~((long)mask & (long)-1);
                    denyMask.Value &= ~((long)mask & (long)-1);
                    break;
                default:
                    allowMask.Value &= ~((long)mask & (long)-1);
                    denyMask.Value |= ((long)mask & (long)-1);
                    break;
            }
        }

        #endregion
    }
}