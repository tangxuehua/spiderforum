using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class UserListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected Pager pager;
        protected CurrentPage currentPage;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.UserAdmin))
            {
                throw new Exception("Access denied.");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            string registeredUserRoleName = Configuration.Instance.RegisteredUserRoleName;
            TRequest<Role> roleRequest = new TRequest<Role>();
            roleRequest.Data.Name.Value = registeredUserRoleName;
            Role registeredUserRole = Engine.GetSingle<Role>(roleRequest);
            if (registeredUserRole != null)
            {
                TRequest<UserAndRole> request = new TRequest<UserAndRole>();
                request.Data.Role.EntityId.Value = registeredUserRole.EntityId.Value;
                request.PageSize = pager.PageSize;
                requestBinders.Add(BinderBuilder.BuildGetListBinder(this, request));
            }
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply);
        }

        #region Ajax Methods

        [AjaxMethod]
        public string ResetUserPassword(int userId)
        {
            ForumUser currentUser = UserManager.GetUser(userId) as ForumUser;

            if (currentUser == null)
            {
                throw new Exception("����ǰҪ����������û������ڣ�");
            }

            Member member = MemberManager.GetFirstMemberByMemberId(currentUser.MemberId.Value.ToString());
            if (member == null)
            {
                throw new Exception("����ǰҪ����������û������ڣ�");
            }

            return MemberManager.ResetMemberPassword(member);
        }

        [AjaxMethod]
        public void LockUser(int userId)
        {
            ForumUser currentUser = UserManager.GetUser(userId) as ForumUser;

            if (currentUser == null)
            {
                throw new Exception("����ǰҪ������û������ڣ�");
            }

            currentUser.UserStatus.Value = (int)UserStatus.Locked;

            Engine.Update(currentUser);
        }

        [AjaxMethod]
        public void UnLockUser(int userId)
        {
            ForumUser currentUser = UserManager.GetUser(userId) as ForumUser;

            if (currentUser == null)
            {
                throw new Exception("����ǰҪ�ⶳ���û������ڣ�");
            }

            currentUser.UserStatus.Value = (int)UserStatus.Normal;

            Engine.Update(currentUser);
        }

        [AjaxMethod]
        public void DeleteUser(int userId)
        {
            ForumUser currentUser = UserManager.GetUser(userId) as ForumUser;

            if (currentUser == null)
            {
                throw new Exception("����ǰҪɾ�����û������ڣ�");
            }

            MemberManager.Delete(currentUser.MemberId.Value, null);
        }
        [AjaxMethod]
        public void DeleteUsers(string items)
        {
            if (string.IsNullOrEmpty(items))
            {
                return;
            }
            int entityId = 0;
            foreach (string item in items.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
            {
                entityId = Globals.ChangeType<int>(item);
                if (entityId > 0)
                {
                    DeleteUser(entityId);
                }
            }
        }

        #endregion

        private void BindData(Reply reply)
        {
            list.DataSource = Engine.GetInnerList<ForumUser>(reply.EntityList, "User");
            list.DataBind();

            if (pager != null)
            {
                pager.TotalRecords = reply.TotalRecords;
                if (currentPage != null)
                {
                    currentPage.TotalRecords = pager.TotalRecords;
                    currentPage.TotalPages = pager.TotalPages;
                    currentPage.PageIndex = pager.PageIndex;
                }
            }
        }
    }
}