using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;
using System.Collections.Generic;

namespace Forum.Business
{
    public class DefaultPage : ForumBasePage
    {
        #region Protected Members

        protected NoneStateRepeater list;
        protected AjaxPager pagerUp;
        protected AjaxPager pagerDown;
        private HttpContext context = HttpContext.Current;

        #endregion

        #region Public Properties

        public int SectionId
        {
            get
            {
                int? sectionId = UrlManager.Instance.GetParameterValue<int?>(ForumParameterName.SectionId);
                if (sectionId.HasValue)
                {
                    return sectionId.Value;
                }
                return 0;
            }
        }
        public string SectionAdminUsers
        {
            get
            {
                if (SectionId > 0)
                {
                    Role sectionAdminRole = RoleManager.GetRole(ForumConfiguration.Instance.ForumSectionAdminRoleName);
                    if (sectionAdminRole != null)
                    {
                        TRequest<SectionRoleAndUser> request = new TRequest<SectionRoleAndUser>();
                        request.Data.SectionRoleUser.SectionId.Value = SectionId;
                        request.Data.SectionRoleUser.RoleId.Value = sectionAdminRole.EntityId.Value;
                        TEntityList<SectionRoleAndUser> sectionAdminUsers = Engine.GetAll<SectionRoleAndUser>(request);
                        List<string> userNames = new List<string>();
                        foreach (SectionRoleAndUser sectionRoleAndUser in sectionAdminUsers)
                        {
                            userNames.Add(sectionRoleAndUser.User.NickName.Value);
                        }
                        return string.Join(",", userNames.ToArray());
                    }
                }
                return string.Empty;
            }
        }

        #endregion

        #region Public Methods

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetListBinder(this, CreateThreadRequest(pagerUp.PageIndex, pagerUp.PageSize)));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply, pagerUp.PageIndex, pagerUp.PageSize);
        }
        [AjaxMethod]
        public ListManageAjaxData RefreshList(int pageIndex)
        {
            BindData(Engine.GetEntityList(CreateThreadRequest(pageIndex, pagerUp.PageSize)), pageIndex, pagerUp.PageSize);

            ListManageAjaxData result = new ListManageAjaxData();
            result.ListContent = Globals.RenderControl(list);
            result.PagingContent = Globals.RenderControl(pagerUp);

            return result;
        }

        #endregion

        #region Private Methods

        private ThreadRequest CreateThreadRequest(int pageIndex, int pageSize)
        {
            int threadReleaseStatus = GetValue<int>(ForumParameterName.ThreadReleaseStatus);
            ThreadRequest request = new ThreadRequest();
            request.PageIndex = pageIndex;
            request.PageSize = pageSize;
            request.CommandIdent = ForumCommandIdent.GetThreads;
            request.Data.SectionId.Value = GetValue<int>(ForumParameterName.SectionId);
            request.Data.ThreadStatus.Value = GetValue<int>(ForumParameterName.ThreadStatus);
            request.Data.ThreadReleaseStatus.Value = threadReleaseStatus == 0 ? (int)ThreadReleaseStatus.Open : threadReleaseStatus;
            request.OrderField.Value = GetValue<int>(ForumParameterName.OrderType);
            return request;
        }
        private void BindData(Reply reply, int pageIndex, int pageSize)
        {
            EntityList threadList = Globals.FixLastPageBug(pageIndex, pageSize, reply.TotalRecords, reply.EntityList);
            list.DataSource = threadList;
            list.DataBind();

            pagerUp.TotalRecords = reply.TotalRecords;
            pagerUp.PageIndex = pageIndex;

            pagerDown.TotalRecords = reply.TotalRecords;
            pagerDown.PageIndex = pageIndex;
        }

        #endregion
    }
}