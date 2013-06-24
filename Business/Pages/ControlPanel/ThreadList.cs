using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class ThreadListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected Pager pager;
        protected CurrentPage currentPage;

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            ThreadRequest request = new ThreadRequest();
            request.CommandIdent = ForumCommandIdent.GetThreads;
            request.PageSize = pager.PageSize;
            request.Data.SectionId.Value = GetValue<int>(ForumParameterName.SectionId);
            request.OrderField.Value = (int)ThreadOrderType.UpdateDate;
            requestBinders.Add(BinderBuilder.BuildGetListBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply);
        }

        #region Ajax Methods

        [AjaxMethod]
        public void DeleteThread(int threadId)
        {
            Thread thread = Engine.Get<Thread>(threadId);
            if (thread == null)
            {
                throw new Exception("当前要删除的帖子不存在！");
            }
            if (!ValidatePermission(PermissionType.DeleteThread, thread))
            {
                throw new Exception("您没有删除帖子的权限！");
            }

            Section section = Engine.Get<Section>(thread.SectionId.Value);

            //检查版块是否存在
            if (section == null)
            {
                throw new Exception("当前帖子所属的版块已经被删除了！");
            }

            section.TotalThreads.Value = section.TotalThreads.Value - 1 > 0 ? section.TotalThreads.Value - 1 : 0;

            TRequest<Post> postRequest = new TRequest<Post>();
            postRequest.Data.ThreadId.Value = threadId;

            Engine.Executes(BinderBuilder.BuildUpdateBinder(section), BinderBuilder.BuildDeleteListBinder(postRequest), BinderBuilder.BuildDeleteBinder<Thread>(threadId));
        }
        [AjaxMethod]
        public void DeleteThreads(string items)
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
                    DeleteThread(entityId);
                }
            }
        }

        #endregion

        private void BindData(Reply reply)
        {
            list.DataSource = Globals.FixLastPageBug(pager.PageIndex, pager.PageSize, reply.TotalRecords, reply.EntityList);
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