using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class MyThreadEditPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedEditor bodyEditor;

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

            Engine.Update(thread);

            return UrlManager.Instance.FormatUrl("mythreadlist", thread.AuthorId.Value);
        }

        private void BindData(Thread thread)
        {
            if (thread != null)
            {
                if (!ValidatePermission(PermissionType.EditThread, thread))
                {
                    throw new Exception("Access denied.");
                }
                subjectTextBox.Value = thread.Subject.Value;
                bodyEditor.Value = thread.Body.Value;
            }
        }
    }
}