using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class CreateThreadPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;
        protected ValuedTextBox threadMarksTextBox;
        protected ValuedEditor bodyEditor;

        protected override void OnFirstLoad()
        {
            int sectionId = GetValue<int>(ForumParameterName.SectionId);
            if (CurrentUser.IsAnonymous)
            {
                Response.Redirect(SiteUrls.Instance.GetLoginUrl(SiteUrls.Instance.GetAddThreadUrl(sectionId)));
                return;
            }
            if (sectionId <= 0)
            {
                throw new Exception("发帖请到小类版块！");
            }
            if (!ValidatePermission(PermissionType.CreateThread))
            {
                throw new Exception("您没有发帖的权限！");
            }
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public string Save()
        {
            Section section = Engine.Get<Section>(GetValue<int>(ForumParameterName.SectionId));

            //检查版块是否存在
            if (section == null)
            {
                throw new Exception("当前版块可能已经被删除了！");
            }

            Thread thread = new Thread();

            //设置帖子属性
            thread.GroupId.Value = section.GroupId.Value;
            thread.SectionId.Value = section.EntityId.Value;
            thread.Subject.Value = subjectTextBox.Value;
            thread.Body.Value = bodyEditor.Value;
            thread.ThreadStatus.Value = (int)ThreadStatus.Normal;
            thread.ThreadReleaseStatus.Value = (int)ThreadReleaseStatus.Open;
            thread.AuthorId.Value = CurrentUser.EntityId.Value;
            thread.Author.Value = CurrentUser.NickName.Value == null ? "" : CurrentUser.NickName.Value;
            thread.CreateDate.Value = DateTime.Now;
            thread.UpdateDate.Value = DateTime.Now;
            thread.StickDate.Value = DateTime.Parse("1753-01-01");

            //更新版块总帖子数
            section.TotalThreads.Value = section.TotalThreads.Value + 1;

            //扣除当前用户的总积分数
            if (CurrentUser.TotalMarks.Value < thread.ThreadMarks.Value)
            {
                throw new Exception("您当前的论坛积分不够！");
            }
            CurrentUser.TotalMarks.Value -= thread.ThreadMarks.Value;

            //执行所有操作
            Engine.Executes(BinderBuilder.BuildUpdateBinder(section), BinderBuilder.BuildCreateBinder(thread), BinderBuilder.BuildUpdateBinder(CurrentUser));

            return SiteUrls.Instance.GetSectionThreadsUrl(section.EntityId.Value);
        }
    }
}