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
                throw new Exception("�����뵽С���飡");
            }
            if (!ValidatePermission(PermissionType.CreateThread))
            {
                throw new Exception("��û�з�����Ȩ�ޣ�");
            }
        }

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public string Save()
        {
            Section section = Engine.Get<Section>(GetValue<int>(ForumParameterName.SectionId));

            //������Ƿ����
            if (section == null)
            {
                throw new Exception("��ǰ�������Ѿ���ɾ���ˣ�");
            }

            Thread thread = new Thread();

            //������������
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

            //���°����������
            section.TotalThreads.Value = section.TotalThreads.Value + 1;

            //�۳���ǰ�û����ܻ�����
            if (CurrentUser.TotalMarks.Value < thread.ThreadMarks.Value)
            {
                throw new Exception("����ǰ����̳���ֲ�����");
            }
            CurrentUser.TotalMarks.Value -= thread.ThreadMarks.Value;

            //ִ�����в���
            Engine.Executes(BinderBuilder.BuildUpdateBinder(section), BinderBuilder.BuildCreateBinder(thread), BinderBuilder.BuildUpdateBinder(CurrentUser));

            return SiteUrls.Instance.GetSectionThreadsUrl(section.EntityId.Value);
        }
    }
}