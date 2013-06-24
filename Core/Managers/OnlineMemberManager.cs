using System;
using System.Collections;
using System.Web;

namespace System.Web.Core
{
    public class OnlineMemberManager
    {
        #region Private Members

        private static OnlineMemberManager instance = new OnlineMemberManager();
        private Hashtable members = new Hashtable();
        private Hashtable guests = new Hashtable();

        #endregion

        #region Constructors.

        private OnlineMemberManager()
        {

        }

        #endregion

        #region Singleton Instance

        public static OnlineMemberManager Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Public Properties

        public Hashtable Members
        {
            get
            {
                return members;
            }
        }
        public Hashtable Guests
        {
            get
            {
                return guests;
            }
        }

        #endregion

        #region Public Methods

        public bool IsMemberOnline(Guid memberId)
        {
            return members.ContainsKey(memberId);
        }
        public void TrackingUser(string lastVisitUrl)
        {
            HttpContext context = HttpContext.Current;
            OnlineMember member = null;

            if (context.User.Identity.IsAuthenticated)
            {
                member = new OnlineMember(new Guid(context.User.Identity.Name));
            }
            else
            {
                member = new OnlineMember(System.Web.Core.MemberManager.GetAnonymousMemberId(), true);
            }

            member.LastVisitUrl = lastVisitUrl;
            Insert(member);
        }
        public void RefreshMembers(int minutes)
        {
            lock (members.SyncRoot)
            {
                DateTime filter = DateTime.Now.AddMinutes(-1 * minutes);
                ArrayList al = new ArrayList(members.Count);
                foreach (OnlineMember user in members.Values)
                {
                    if (user.LastActiveTime >= filter)
                    {
                        al.Add(user);
                    }
                }

                members.Clear();
                foreach (OnlineMember user in al)
                {
                    members[user.MemberId] = user;
                }
            }
        }
        public void RefreshGuests(int minutes)
        {
            lock (guests.SyncRoot)
            {
                DateTime filter = DateTime.Now.AddMinutes(-1 * minutes);
                ArrayList al = new ArrayList(guests.Count);
                foreach (OnlineMember user in guests.Values)
                {
                    if (user.LastActiveTime >= filter)
                    {
                        al.Add(user);
                    }
                }

                guests.Clear();
                foreach (OnlineMember user in al)
                {
                    guests[user.MemberId] = user;
                }
            }
        }
        public void RefreshAllUsers(int minutes)
        {
            RefreshMembers(minutes);
            RefreshGuests(minutes);
        }

        #endregion

        #region Private Methods

        private void Insert(OnlineMember onlineUser)
        {
            if (onlineUser.IsAnonymous)
            {
                lock (guests.SyncRoot)
                {
                    guests[onlineUser.MemberId] = onlineUser;
                }
            }
            else
            {
                lock (members.SyncRoot)
                {
                    members[onlineUser.MemberId] = onlineUser;
                }
            }
        }

        #endregion
    }
}
