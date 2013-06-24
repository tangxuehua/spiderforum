using System;
using System.ComponentModel;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Core;
using Forum.Business;

namespace Forum.Business
{
    public class ThreadBreadCrumb : BaseBreadCrumb
    {
        private int sectionId = UrlManager.Instance.GetParameterValue<int>(ForumParameterName.SectionId);
        private int threadId = UrlManager.Instance.GetParameterValue<int>(ForumParameterName.ThreadId);

        protected override void GetCrumbs(Queue<Control> crumbs)
        {
            crumbs.Enqueue(CreateAnchor("论坛首页", SiteUrls.Instance.Home));
            if (threadId > 0)
            {
                Thread thread = Engine.Get<Thread>(threadId);
                if (thread != null)
                {
                    Section section = Engine.Get<Section>(thread.SectionId.Value);
                    if (section != null)
                    {
                        crumbs.Enqueue(CreateAnchor(Page.Server.HtmlEncode(section.Subject.Value), SiteUrls.Instance.GetSectionThreadsUrl(section.EntityId.Value)));
                    }
                    crumbs.Enqueue(CreateAnchor(Page.Server.HtmlEncode(thread.Subject.Value), SiteUrls.Instance.GetThreadUrl(thread.EntityId.Value)));
                }
            }
            else if (sectionId > 0)
            {
                Section section = Engine.Get<Section>(sectionId);
                if (section != null)
                {
                    crumbs.Enqueue(CreateAnchor(Page.Server.HtmlEncode(section.Subject.Value), SiteUrls.Instance.GetSectionThreadsUrl(section.EntityId.Value)));
                }
            }
        }
    }
}