using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class ThreadEditPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedEditor bodyEditor;

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.EditThread))
            {
                throw new Exception("Access denied.");
            }
        }
        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new ThreadRequest(GetValue<int>(ForumParameterName.ThreadId))));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.Entity as Thread);
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public string Save()
        {
            Thread thread = Engine.Get<Thread>(new ThreadRequest(GetValue<int>(ForumParameterName.ThreadId)));
            if (thread == null)
            {
                throw new Exception("当前修改的帖子不存在！");
            }

            thread.Subject.Value = subjectTextBox.Value;
            thread.Body.Value = bodyEditor.Value;

            if (thread.ThreadStatus.Value == (int)ThreadStatus.Stick)
            {
                thread.StickDate.Value = DateTime.Now;
            }
            else
            {
                thread.StickDate.Value = DateTime.Parse("1753-01-01");
            }

            Engine.Update(thread);

            return UrlManager.Instance.FormatUrl("thread_list", thread.SectionId.Value);
        }

        private void BindData(Thread thread)
        {
            if (thread != null)
            {
                subjectTextBox.Value = thread.Subject.Value;
                bodyEditor.Value = thread.Body.Value;
            }
        }
    }
}