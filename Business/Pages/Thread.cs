using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class ThreadPage : ForumBasePage
    {
        #region Private Members

        private ForumUser threadUser;
        private bool canSetAsStick = false;
        private bool canSetAsRecommended = false;
        private bool isThreadClosed = false;
        private bool canCloseThread = false;
        private bool isThreadExist = false;

        #endregion

        #region Protected Members

        protected NoneStateRepeater list;
        protected NoneStateRepeater postList;

        protected bool CanSetAsStick
        {
            get
            {
                return canSetAsStick;
            }
        }
        protected bool CanSetAsRecommended
        {
            get
            {
                return canSetAsRecommended;
            }
        }
        protected bool IsThreadClosed
        {
            get
            {
                return isThreadClosed;
            }
        }
        protected bool CanCloseThread
        {
            get
            {
                return canCloseThread;
            }
        }
        protected ForumUser ThreadUser
        {
            get
            {
                return threadUser;
            }
        }
        protected bool IsThreadExist
        {
            get
            {
                return isThreadExist;
            }
        }

        #endregion

        #region Override Members

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.View))
            {
                throw new Exception("您没有查看帖子的权限！");
            }
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
            Thread thread = requestBinders[0].Reply.Entity as Thread;

            if (thread != null)
            {
                canSetAsStick = ValidatePermission(PermissionType.StickThread, thread);
                canSetAsRecommended = ValidatePermission(PermissionType.ModifyThreadStatus, thread);
                isThreadClosed = thread.ThreadReleaseStatus.Value == (int)ThreadReleaseStatus.Close;
                canCloseThread = !IsThreadClosed && ValidatePermission(PermissionType.CloseThread, thread);
                isThreadExist = true;

                threadUser = UserManager.GetUser(thread.AuthorId.Value) as ForumUser;

                BindThreadRepeater(thread);
                thread.TotalViews.Value++;
                Engine.Update(thread);

                BindPostsRepeater(requestBinders[1].Reply.EntityList);
            }
            else
            {
                throw new Exception("您访问的帖子不存在，可能已经被删除。");
            }
        }

        #endregion

        #region Ajax Methods

        [AjaxMethod]
        public void SetAsStick()
        {
            if (!ValidatePermission(PermissionType.StickThread))
            {
                throw new Exception("您没有置顶帖子的权限！");
            }
            Thread thread = Engine.Get<Thread>(UrlManager.Instance.GetParameterValue<int>(ForumParameterName.ThreadId));
            if (thread == null)
            {
                throw new Exception("您当前要操作的帖子不存在！");
            }

            thread.ThreadStatus.Value = (int)ThreadStatus.Stick;
            thread.StickDate.Value = DateTime.Now;

            Engine.Update(thread);
        }
        [AjaxMethod]
        public void SetAsRecommended()
        {
            if (!ValidatePermission(PermissionType.ModifyThreadStatus))
            {
                throw new Exception("您没有修改帖子状态的权限！");
            }

            Thread thread = Engine.Get<Thread>(UrlManager.Instance.GetParameterValue<int>(ForumParameterName.ThreadId));
            if (thread == null)
            {
                throw new Exception("您当前要操作的帖子不存在！");
            }
            if (thread.ThreadStatus.Value == (int)ThreadStatus.Recommended)
            {
                throw new Exception("您当前要操作的帖子已经是推荐帖子！");
            }

            thread.StickDate.Value = DateTime.Parse("1753-01-01");
            thread.ThreadStatus.Value = (int)ThreadStatus.Recommended;

            Engine.Update(thread);
        }
        [AjaxMethod]
        public ThreadResult CreatePost(string content)
        {
            if (CurrentUser.IsAnonymous)
            {
                throw new Exception("请登陆后再回复帖子！");
            }

            Thread thread = Engine.Get<Thread>(new ThreadRequest(GetValue<int>(ForumParameterName.ThreadId)));
            if (thread == null)
            {
                throw new Exception("您要回复的帖子不存在！");
            }

            Post post = new Post();

            post.GroupId.Value = thread.GroupId.Value;
            post.SectionId.Value = thread.SectionId.Value;
            post.ThreadId.Value = thread.EntityId.Value;
            post.Body.Value = content;
            post.AuthorId.Value = CurrentUser.EntityId.Value;
            post.CreateDate.Value = DateTime.Now;

            thread.TotalPosts.Value = thread.TotalPosts.Value + 1;
            thread.MostRecentReplierId.Value = CurrentUser.EntityId.Value;
            thread.MostRecentReplierName.Value = CurrentUser.NickName.Value;
            thread.UpdateDate.Value = post.CreateDate.Value;

            Engine.Executes(BinderBuilder.BuildCreateBinder(post), BinderBuilder.BuildUpdateBinder(thread));

            TRequest<PostAndUser> request = new TRequest<PostAndUser>();
            request.Data.Post.ThreadId.Value = GetValue<int>(ForumParameterName.ThreadId);
            request.OrderFields.Add(new OrderField("EntityId", true));
            BindPostsRepeater(Engine.GetAll<PostAndUser>(request));

            ThreadResult threadResult = new ThreadResult();
            threadResult.TotalPosts = thread.TotalPosts.Value.ToString();
            threadResult.PostList = Globals.RenderControl(postList);

            return threadResult;
        }

        #endregion

        #region Private Methods

        private void BindThreadRepeater(Thread thread)
        {
            List<Thread> threads = new List<Thread>();
            threads.Add(thread);

            list.DataSource = threads;
            list.DataBind();
        }
        private void BindPostsRepeater(EntityList posts)
        {
            for (int i = 0; i < posts.Count; i++)
            {
                ((PostAndUser)posts[i]).Post.PostIndex = i + 1;
            }

            postList.DataSource = posts;
            postList.DataBind();
        }

        #endregion
    }

    public class ThreadResult
    {
        private string totalPosts = null;
        private string postList = null;

        public string TotalPosts
        {
            get
            {
                return totalPosts;
            }
            set
            {
                totalPosts = value;
            }
        }
        public string PostList
        {
            get
            {
                return postList;
            }
            set
            {
                postList = value;
            }
        }
    }
}