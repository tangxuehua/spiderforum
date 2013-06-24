using System;
using System.IO;
using System.Web.Core;

namespace Forum.Business
{
    public class SiteUrls
    {
        #region Singleton Instance

        private static UrlManager urlManager = UrlManager.Instance;
        private static SiteUrls instance = new SiteUrls();

        private SiteUrls()
        {
        }
        public static SiteUrls Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Private Methods

        private string FormatUrl(string name)
        {
            return urlManager.FormatUrl(name);
        }
        private string FormatUrl(string name, params object[] parameters)
        {
            return urlManager.FormatUrl(name, parameters);
        }

        #endregion

        public string Home
        {
            get
            {
                return FormatUrl("home");
            }
        }
        public string Register
        {
            get
            {
                return FormatUrl("register");
            }
        }
        public string Login
        {
            get
            {
                return FormatUrl("login");
            }
        }
        public string GetLoginUrl(string returnUrl)
        {
            return FormatUrl("loginWithReturnUrl", returnUrl);
        }
        public string Logout
        {
            get
            {
                return FormatUrl("logout");
            }
        }
        public string ControlPanel
        {
            get
            {
                return FormatUrl("controlpanel");
            }
        }

        public string GetViewExceptionLogUrl(int exceptionLogId)
        {
            return FormatUrl("exceptionlog_view", exceptionLogId);
        }

        public string GetRoleAddUrl()
        {
            return FormatUrl("role_add");
        }
        public string GetRoleEditUrl(int roleId)
        {
            return FormatUrl("role_edit", roleId);
        }
        public string GetRoleListUrl()
        {
            return FormatUrl("role_list");
        }
        public string GetUserRolesUrl(int userId)
        {
            return FormatUrl("userroles", userId);
        }

        public string GetGroupAddUrl()
        {
            return FormatUrl("group_add");
        }
        public string GetGroupEditUrl(int groupId)
        {
            return FormatUrl("group_edit", groupId);
        }
        public string GetGroupListUrl()
        {
            return FormatUrl("group_list");
        }

        public string GetSectionAddUrl(int groupId)
        {
            return FormatUrl("section_add", groupId);
        }
        public string GetSectionEditUrl(int sectionId)
        {
            return FormatUrl("section_edit", sectionId);
        }
        public string GetSectionListUrl(int groupId)
        {
            return FormatUrl("section_list", groupId);
        }
        public string GetSectionAdminsUrl(int sectionId, int roleId)
        {
            return FormatUrl("sectionadmins", sectionId, roleId);
        }
        public string GetCreateTestThreadsUrl(int sectionId)
        {
            return FormatUrl("section_createtestthreads", sectionId);
        }

        public string GetThreadAddUrl(int sectionId)
        {
            return FormatUrl("thread_add", sectionId);
        }
        public string GetThreadEditUrl(int threadId)
        {
            return FormatUrl("thread_edit", threadId);
        }
        public string GetThreadListUrl(int sectionId)
        {
            return FormatUrl("thread_list", sectionId);
        }
        public string GetMyThreadListUrl(int userId)
        {
            return FormatUrl("mythreadlist", userId);
        }
        public string GetMyReplyThreadListUrl(int userId)
        {
            return FormatUrl("myreplythreadlist", userId);
        }
        public string GetMyOpenThreadListUrl(int userId)
        {
            return FormatUrl("myopenthreadlist", userId);
        }
        public string GetMyCloseThreadListUrl(int userId)
        {
            return FormatUrl("myclosethreadlist", userId);
        }
        public string GetMyThreadEditUrl(int threadId)
        {
            return FormatUrl("mythreadedit", threadId);
        }

        public string GetSectionThreadsUrl(int sectionId)
        {
            if (sectionId > 0)
            {
                return FormatUrl("sectionthreads", sectionId);
            }
            else
            {
                return Home;
            }
        }
        public string GetSectionRecommendedThreadsUrl(int sectionId)
        {
            if (sectionId > 0)
            {
                return FormatUrl("sectionrecommendedthreads", sectionId);
            }
            else
            {
                return FormatUrl("recommendedthreads");
            }
        }
        public string GetThreadUrl(int threadId)
        {
            return FormatUrl("thread", threadId);
        }
        public string GetCloseThreadUrl(int threadId)
        {
            return FormatUrl("closethread", threadId);
        }
        public string GetAddThreadUrl(int sectionId)
        {
            return FormatUrl("addthread", sectionId);
        }

        public string GetPostAddUrl(int threadId)
        {
            return FormatUrl("post_add", threadId);
        }
        public string GetPostEditUrl(int postId)
        {
            return FormatUrl("post_edit", postId);
        }
        public string GetPostListUrl(int threadId)
        {
            return FormatUrl("post_list", threadId);
        }
        public string GetMyPostEditUrl(int postId)
        {
            return FormatUrl("mypostedit", postId);
        }
    }
}
