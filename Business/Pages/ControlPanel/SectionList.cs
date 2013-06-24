using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionListPage : ForumBasePage
    {
        protected NoneStateRepeater list;
        protected ValuedDropDownList groupDropDownList;
        protected AjaxPager pager;
        protected CurrentPage currentPage;

        public string AdminUserRoleId
        {
            get
            {
                string s = ViewState["AdminUserRoleId"] as string;
                if (s == null)
                {
                    s = string.Empty;
                }
                return s;
            }
            set
            {
                ViewState["AdminUserRoleId"] = value;
            }
        }

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, new TRequest<Group>()));
            TRequest<Section> request = new TRequest<Section>();
            request.PageSize = pager.PageSize;
            requestBinders.Add(BinderBuilder.BuildGetListBinder(this, request));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindGroupDropDownList(requestBinders[0].Reply.EntityList);
            BindRepeater(requestBinders[1].Reply);
            BindPager(requestBinders[1].Reply.TotalRecords, 1);
        }
        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.SectionAdmin))
            {
                throw new Exception("您没有管理版块或版块组的权限！");
            }
        }

        #region Ajax Methods

        [AjaxMethod]
        public void DeleteSection(int sectionId)
        {
            TRequest<Post> postRequest = new TRequest<Post>();
            postRequest.Data.SectionId.Value = sectionId;
            ThreadRequest threadRequest = new ThreadRequest();
            threadRequest.Data.SectionId.Value = sectionId;
            TRequest<SectionRoleUser> sectionRoleUserRequest = new TRequest<SectionRoleUser>();
            sectionRoleUserRequest.Data.SectionId.Value = sectionId;

            Engine.Executes(BinderBuilder.BuildDeleteListBinder(postRequest), BinderBuilder.BuildDeleteListBinder(threadRequest), BinderBuilder.BuildDeleteListBinder(sectionRoleUserRequest), BinderBuilder.BuildDeleteBinder<Section>(sectionId));
        }
        [AjaxMethod]
        public void DeleteSections(string items)
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
                    DeleteSection(entityId);
                }
            }
        }
        [AjaxMethod]
        public ListManageAjaxData RefreshList(int groupId, int pageIndex)
        {
            TRequest<Section> request = new TRequest<Section>();

            request.Data.GroupId.Value = groupId;
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

        private void BindGroupDropDownList(EntityList groups)
        {
            Group topGroup = new Group();
            topGroup.Subject.Value = "所有版块组";
            groups.Insert(0, topGroup);

            groupDropDownList.DataSource = groups;
            groupDropDownList.DataTextField = "Subject";
            groupDropDownList.DataValueField = "EntityId";
            groupDropDownList.DataBind();
        }
        private void BindRepeater(Reply reply)
        {
            AdminUserRoleId = RoleManager.GetRole(ForumConfiguration.Instance.ForumSectionAdminRoleName).EntityId.Value.ToString();
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