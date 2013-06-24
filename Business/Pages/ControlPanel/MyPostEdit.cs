using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Core;

namespace Forum.Business
{
    public class MyPostEditPage : ForumBasePage
    {
        protected ValuedEditor bodyEditor;

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new TRequest<Post>(GetValue<int>(ForumParameterName.PostId))));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            BindData(requestBinders[0].Reply.Entity as Post);
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            Post post = Engine.Get<Post>(new TRequest<Post>(GetValue<int>(ForumParameterName.PostId)));
            if (post != null)
            {
                post.Body.Value = bodyEditor.Value;
                Engine.Update(post);
            }
        }

        private void BindData(Post post)
        {
            if (post != null)
            {
                if (!ValidatePermission(PermissionType.EditPost, post))
                {
                    throw new Exception("您没有编辑回复的权限！");
                }
                bodyEditor.Value = post.Body.Value;
            }
        }
    }
}