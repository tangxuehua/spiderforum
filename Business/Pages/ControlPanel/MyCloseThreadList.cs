using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Core;
using System;

namespace Forum.Business
{
    public class MyCloseThreadListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected Pager pager;
        protected CurrentPage currentPage;

        protected override void OnFirstLoad()
        {
            throw new Exception("由于结贴功能已关闭，故暂不能访问该页面！");
        }

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            ThreadRequest request = new ThreadRequest();
            request.CommandIdent = ForumCommandIdent.GetThreads;
            request.PageSize = pager.PageSize;
            request.Data.AuthorId.Value = GetValue<int>(ForumParameterName.AuthorId);
            request.Data.ThreadReleaseStatus.Value = GetValue<int>(ForumParameterName.ThreadReleaseStatus);
            requestBinders.Add(BinderBuilder.BuildGetListBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply);
        }

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