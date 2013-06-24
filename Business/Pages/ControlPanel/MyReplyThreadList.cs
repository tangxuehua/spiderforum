using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class MyReplyThreadListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected Pager pager;
        protected CurrentPage currentPage;

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            ThreadRequest request = new ThreadRequest();
            request.CommandIdent = ForumCommandIdent.GetThreads;
            request.PageSize = pager.PageSize;
            request.ReplierId.Value = GetValue<int>(ForumParameterName.ReplierId);
            request.OrderField.Value = (int)ThreadOrderType.UpdateDate;
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