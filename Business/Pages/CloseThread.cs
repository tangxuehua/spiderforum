using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class CloseThreadPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected NoneStateRepeater postList;
        private Thread thread;
        private ForumUser threadUser;

        protected Thread Thread
        {
            get
            {
                return thread;
            }
        }
        protected ForumUser ThreadUser
        {
            get
            {
                return threadUser;
            }
        }

        protected override void OnFirstLoad()
        {
            throw new Exception("暂不提供结贴功能！");
            //if (CurrentUser.IsAnonymous)
            //{
            //    Response.Redirect(SiteUrls.Instance.GetLoginUrl(SiteUrls.Instance.GetCloseThreadUrl(GetValue<int>(ForumParameterName.ThreadId))));
            //}
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new ThreadRequest(GetValue<int>(ForumParameterName.ThreadId))));

            TRequest<PostAndUser> request = new TRequest<PostAndUser>();
            request.Data.Post.ThreadId.Value = GetValue<int>(ForumParameterName.ThreadId);
            request.OrderFields.Add(new OrderField("EntityId", true));
            requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.Entity as Thread, requestBinders[1].Reply.EntityList);
        }

        #region Ajax Methods

        [AjaxMethod]
        public void CloseCurrentThread(int threadId, string userIds, string scores)
        {
            thread = Engine.Get<Thread>(new ThreadRequest(GetValue<int>(ForumParameterName.ThreadId)));

            if (thread == null)
            {
                throw new Exception("您当前要结帖的帖子不存在！");
            }
            if (thread.ThreadReleaseStatus.Value == (int)ThreadReleaseStatus.Close)
            {
                throw new Exception("您当前要结帖的帖子已經結帖！");
            }
            if (!ValidatePermission(PermissionType.CloseThread, thread))
            {
                throw new Exception("您没有结帖的权限！");
            }

            List<RequestBinder> binders = new List<RequestBinder>();

            thread.ThreadReleaseStatus.Value = (int)ThreadReleaseStatus.Close;

            binders.Add(BinderBuilder.BuildUpdateBinder(thread));

            ForumUser threadAuthor = UserManager.GetUser(thread.AuthorId.Value) as ForumUser;
            if (threadAuthor != null)
            {
                threadAuthor.TotalMarks.Value += thread.ThreadMarks.Value / 5;  //结贴返还五分之一的积分
                binders.Add(BinderBuilder.BuildUpdateBinder(threadAuthor));
            }

            Dictionary<string, string> dictionary = ProcessUserScores(userIds, scores);
            foreach (string userIdString in dictionary.Keys)
            {
                ForumUser user = UserManager.GetUser(int.Parse(userIdString)) as ForumUser;
                if (user != null)
                {
                    user.TotalMarks.Value += int.Parse(dictionary[userIdString]);
                    binders.Add(BinderBuilder.BuildUpdateBinder(user));
                }
            }

            Engine.Executes(binders);
        }

        #endregion

        #region Private Methods

        private void BindData(Thread thread, EntityList posts)
        {
            if (thread != null)
            {
                if (!ValidatePermission(PermissionType.CloseThread, thread))
                {
                    throw new Exception("您没有结帖的权限！");
                }

                this.thread = thread;

                List<Thread> threads = new List<Thread>();
                threads.Add(thread);

                threadUser = UserManager.GetUser(thread.AuthorId.Value) as ForumUser;

                list.DataSource = threads;
                list.DataBind();
            }
            if (posts.Count > 0)
            {
                for (int i = 0; i < posts.Count; i++)
                {
                    ((PostAndUser)posts[i]).Post.PostIndex = i + 1;
                }
                postList.DataSource = posts;
                postList.DataBind();
            }
        }
        private Dictionary<string, string> ProcessUserScores(string userIds, string scores)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(userIds) || string.IsNullOrEmpty(scores))
            {
                return dictionary;
            }
            string[] userIdArray = userIds.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string[] scoreArray = scores.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string userId = string.Empty;
            string score = string.Empty;

            if (userIdArray.Length != scoreArray.Length)
            {
                return dictionary;
            }

            for (int index = 0; index < userIdArray.Length; index++)
            {
                userId = userIdArray[index];
                if (!dictionary.ContainsKey(userId))
                {
                    dictionary.Add(userId, scoreArray[index]);
                }
                else
                {
                    dictionary[userId] = ((int)(int.Parse(dictionary[userId]) + int.Parse(scoreArray[index]))).ToString();
                }
            }

            return dictionary;
        }

        #endregion
    }
}