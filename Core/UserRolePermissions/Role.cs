using System;

namespace System.Web.Core
{
    public class Role : Entity
    {
        #region Private Members

        private StringType name = new StringType();
        private StringType description = new StringType();
        private LongType roleType = new LongType();

        #endregion

        #region Public Properties

        public StringType Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public StringType Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        public LongType RoleType
        {
            get
            {
                return roleType;
            }
            set
            {
                roleType = value;
            }
        }

        #endregion

        #region Public Methods

        public bool IsRoleType(long roleType)
        {
            return (RoleType.Value & Convert.ToInt64(roleType.ToString(), 16)) == Convert.ToInt64(roleType.ToString(), 16);
        }

        #endregion
    }
}