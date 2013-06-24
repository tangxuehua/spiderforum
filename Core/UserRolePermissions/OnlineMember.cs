using System;

namespace System.Web.Core
{
    public class OnlineMember
    {
        #region Private Members

        private Guid memberId = Guid.Empty;
        private DateTime lastActiveTime = DateTime.Now;
        private string lastVisitUrl = null;
        private bool isAnonymous = false;

        #endregion

        #region Constructors

        public OnlineMember(Guid memberId)
        {
            this.memberId = memberId;
        }
        public OnlineMember(Guid memberId, bool isAnonymous)
        {
            this.memberId = memberId;
            this.isAnonymous = isAnonymous;
        }

        #endregion

        #region Public Properites

        public Guid MemberId
        {
            get
            {
                return memberId;
            }
        }
        public bool IsAnonymous
        {
            get
            {
                return isAnonymous;
            }
        }
        public DateTime LastActiveTime
        {
            get
            {
                return lastActiveTime;
            }
            set
            {
                lastActiveTime = value;
            }
        }
        public string LastVisitUrl
        {
            get
            {
                return lastVisitUrl;
            }
            set
            {
                lastVisitUrl = value;
            }
        }

        #endregion
    }
}
