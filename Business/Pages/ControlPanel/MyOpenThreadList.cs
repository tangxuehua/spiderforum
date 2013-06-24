using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class MyOpenThreadListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected Pager pager;
        protected CurrentPage currentPage;

        public bool CanEditMyThread
        {
            get
            {
                object o = ViewState["CanEditMyThread"];
                if (o != null)
                {
                    return bool.Parse(o.ToString());
                }
                return false;
            }
            set
            {
                ViewState["CanEditMyThread"] = value;
            }
        }
        public bool CanDeleteMyThread
        {
            get
            {
                object o = ViewState["CanDeleteMyThread"];
                if (o != null)
                {
                    return bool.Parse(o.ToString());
                }
                return false;
            }
            set
            {
                ViewState["CanDeleteMyThread"] = value;
            }
        }

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

        #region Ajax Method

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
            Engine.Delete<Thread>(threadId);
        }

        #endregion

        private void BindData(Reply reply)
        {
            if (reply.EntityList.Count > 0)
            {
                if (ValidatePermission(PermissionType.EditThread, reply.EntityList[0]))
                {
                    CanEditMyThread = true;
                }
                if (ValidatePermission(PermissionType.DeleteThread, reply.EntityList[0]))
                {
                    CanDeleteMyThread = true;
                }
            }

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