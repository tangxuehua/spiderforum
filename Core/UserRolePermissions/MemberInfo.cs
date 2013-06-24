using System;
using System.Collections;

namespace System.Web.Core
{
    public class MemberInfo
    {
        #region Private Members

        private Guid memberId;
        private Hashtable memberAttributes;

        #endregion

        #region Public Properties

        public Guid MemberId
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
        public Hashtable MemberAttributes
        {
            get
            {
                return memberAttributes;
            }
            set
            {
                memberAttributes = value;
            }
        }

        #endregion
    }
}