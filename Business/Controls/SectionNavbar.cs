using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionNavbar : ForumUserControl
    {
        protected NoneStateRepeater tabRepeater;

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    //tabRepeater.ItemDataBound += new RepeaterItemEventHandler(tabRepeater_ItemDataBound);
        //}
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            //左侧导航修改为只显示一层类别
            //TRequest<Group> groupRequest = new TRequest<Group>();
            //groupRequest.Data.Enabled.Value = (int)EnableStatus.Enable;
            //requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, groupRequest));

            TRequest<Section> sectionRequest = new TRequest<Section>();
            sectionRequest.Data.Enabled.Value = (int)EnableStatus.Enable;
            requestBinders.Add(BinderBuilder.BuildGetAllBinder(this, sectionRequest));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.EntityList);
        }

        #region Private Methods

        private void BindData(EntityList sectionList)
        {
            if (tabRepeater != null)
            {
                this.tabRepeater.DataSource = sectionList;
                this.tabRepeater.DataBind();
            }
        }
        //private void tabRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    Group group = e.Item.DataItem as Group;
        //    NoneStateRepeater subTabRepeater = e.Item.FindControl("subTabRepeater") as NoneStateRepeater;

        //    if (subTabRepeater != null)
        //    {
        //        subTabRepeater.DataSource = GetSectionList(group.EntityId.Value);
        //        subTabRepeater.DataBind();
        //    }
        //}
        //private EntityList GetSectionList(int groupId)
        //{
        //    EntityList currentSectionList = new EntityList();
        //    foreach (Section section in sectionList)
        //    {
        //        if (section.GroupId.Value == groupId)
        //        {
        //            currentSectionList.Add(section);
        //        }
        //    }
        //    return currentSectionList;
        //}

        #endregion
    }
}