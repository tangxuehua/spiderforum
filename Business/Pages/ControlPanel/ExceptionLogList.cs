using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;
using System.Text;

namespace Forum.Business
{
    public class ExceptionLogListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected AjaxPager pager;
        protected CurrentPage currentPage;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.ExceptionLogAdmin))
            {
                throw new Exception("您没有错误日志管理的权限！");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            TRequest<ExceptionLog> request = new TRequest<ExceptionLog>();
            request.PageSize = pager.PageSize;
            requestBinders.Add(BinderBuilder.BuildGetListBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindRepeater(requestBinders[0].Reply);
            BindPager(requestBinders[0].Reply.TotalRecords, 1);
        }

        #region Ajax Methods

        [AjaxMethod]
        public void DeleteExceptionLog(int entityId)
        {
            Engine.Delete<ExceptionLog>(entityId);
        }
        [AjaxMethod]
        public void DeleteExceptionLogs(string items)
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
                    DeleteExceptionLog(entityId);
                }
            }
        }
        [AjaxMethod]
        public ListManageAjaxData RefreshList(int pageIndex)
        {
            TRequest<ExceptionLog> request = new TRequest<ExceptionLog>();

            request.PageIndex = pageIndex;
            request.PageSize = pager.PageSize;

            Reply reply = Engine.GetEntityList(request);

            BindRepeater(reply);
            BindPager(reply.TotalRecords, pageIndex);

            ListManageAjaxData result = new ListManageAjaxData();
            result.ListContent = Globals.RenderControl(list);
            result.PagingContent = Globals.RenderControl(currentPage) + Globals.RenderControl(pager);

            return result;
        }

        #endregion

        private void BindRepeater(Reply reply)
        {
            list.DataSource = reply.EntityList;
            list.DataBind();
        }
        private void BindPager(int totalRecords, int pageIndex)
        {
            if (pager != null)
            {
                pager.TotalRecords = totalRecords;
                pager.PageIndex = pageIndex;
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