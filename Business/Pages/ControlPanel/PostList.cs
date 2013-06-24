using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class PostListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected Pager pager;
        protected CurrentPage currentPage;

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            TRequest<Post> request = new TRequest<Post>();
            request.PageSize = pager.PageSize;
            request.Data.ThreadId.Value = GetValue<int>(ForumParameterName.ThreadId);
            request.OrderFields.Add(new OrderField("EntityId", true));
            requestBinders.Add(BinderBuilder.BuildGetListBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply);
        }

        #region Ajax Methods

        [AjaxMethod]
        public void DeletePost(int postId)
        {
            if (!ValidatePermission(PermissionType.DeletePost))
            {
                throw new Exception("您没有删除回复的权限！");
            }
            Post post = Engine.Get<Post>(postId);
            post.Body.Value = "该回复已被删除";
            Engine.Update(post);
        }
        [AjaxMethod]
        public void DeletePosts(string items)
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
                    DeletePost(entityId);
                }
            }
        }

        #endregion

        private void BindData(Reply reply)
        {
            list.DataSource = reply.EntityList;
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