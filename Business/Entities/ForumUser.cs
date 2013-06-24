using System;
using System.Web;
using System.Web.Core;

namespace Forum.Business
{
    public class ForumUser : User
    {
        #region Private Members

        private StringType nickName = new StringType();
        private IntType userStatus = new IntType();
        private IntType totalMarks = new IntType();

        #endregion

        #region Public Properties

        public StringType NickName
        {
            get
            {
                return nickName;
            }
            set
            {
                nickName = value;
            }
        }
        [Validator(typeof(UserStatusValidator))]
        public IntType UserStatus
        {
            get
            {
                return userStatus;
            }
            set
            {
                userStatus = value;
            }
        }
        public IntType TotalMarks
        {
            get
            {
                return totalMarks;
            }
            set
            {
                totalMarks = value;
            }
        }

        #endregion
    }
}