using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;
using System.Text;

namespace Forum.Business
{
    public class GroupListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected AjaxPager pager;
        protected CurrentPage currentPage;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdmin))
            {
                throw new Exception("您没有修改版块或版块组的权限！");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            TRequest<Group> request = new TRequest<Group>();
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
        public void DeleteGroup(int entityId)
        {
            List<RequestBinder> binders = new List<RequestBinder>();

            TRequest<Post> postRequest = new TRequest<Post>();
            postRequest.Data.GroupId.Value = entityId;
            binders.Add(BinderBuilder.BuildDeleteListBinder(postRequest));

            ThreadRequest threadRequest = new ThreadRequest();
            threadRequest.Data.GroupId.Value = entityId;
            binders.Add(BinderBuilder.BuildDeleteListBinder(threadRequest));

            TRequest<Section> sectionRequest = new TRequest<Section>();
            sectionRequest.Data.GroupId.Value = entityId;
            binders.Add(BinderBuilder.BuildDeleteListBinder(sectionRequest));

            TRequest<Section> groupSectionsRequest = new TRequest<Section>();
            groupSectionsRequest.Data.GroupId.Value = entityId;
            List<string> sectionIdList = new List<string>();
            foreach (Section section in Engine.GetAll<Section>(groupSectionsRequest))
            {
                sectionIdList.Add(section.EntityId.Value.ToString());
            }
            if (sectionIdList.Count > 0)
            {
                TRequest<SectionRoleUser> sectionRoleUserRequest = new TRequest<SectionRoleUser>();
                sectionRoleUserRequest.Data.SectionId.Condition = new Condition(typeof(StringValidator), "In", "(" + string.Join(",", sectionIdList.ToArray()) + ")");
                binders.Add(BinderBuilder.BuildDeleteListBinder(sectionRoleUserRequest)); 
            }

            binders.Add(BinderBuilder.BuildDeleteBinder<Group>(entityId));

            Engine.Executes(binders);
        }
        [AjaxMethod]
        public void DeleteGroups(string items)
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
                    DeleteGroup(entityId);
                }
            }
        }
        [AjaxMethod]
        public ListManageAjaxData RefreshList(int pageIndex)
        {
            TRequest<Group> request = new TRequest<Group>();

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