using System;

namespace System.Web.Core
{
    public sealed class Member : Entity
    {
        #region Private Members

        private GuidType memberId = new GuidType();
        private StringType memberName = new StringType();
        private StringType password = new StringType();
        private IntType passwordFormat = new IntType();
        private StringType passwordSalt = new StringType();

        #endregion

        #region Public Properties

        public GuidType MemberId
        {
            get
            {
                return memberId;
            }
            set
            {
                memberId = value;
            }
        }
        public StringType MemberName
        {
            get
            {
                return memberName;
            }
            set
            {
                memberName = value;
            }
        }
        public StringType Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public IntType PasswordFormat
        {
            get
            {
                return passwordFormat;
            }
            set
            {
                passwordFormat = value;
            }
        }
        public StringType PasswordSalt
        {
            get
            {
                return passwordSalt;
            }
            set
            {
                passwordSalt = value;
            }
        }

        #endregion
    }
}