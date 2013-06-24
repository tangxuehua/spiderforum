using System;
using System.Web;

namespace System.Web.Core 
{
    public class HttpModule : IHttpModule
    {
        #region Implementation of IHttpModule

        public void Init(HttpApplication application) 
        {
            application.AuthenticateRequest += new EventHandler(Application_AuthenticateRequest);
            application.AuthorizeRequest += new EventHandler(Application_AuthorizeRequest);
        }

        public void Dispose()
        {
        }

        #endregion

        #region Event Handlers

        private void Application_AuthenticateRequest(Object source, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.LocalPath.ToLower().EndsWith(".aspx"))
            {
                SetCurrentUser();
            }
        }
        private void Application_AuthorizeRequest(Object source, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.LocalPath.ToLower().EndsWith(".aspx"))
            {
                ReWriteUrl();
            }
        }

        #endregion

        #region Private Methods

        private void ReWriteUrl()
        {
            HttpContext context = HttpContext.Current;
            UrlReWriter urlReWriter = UrlReWriter.Instance;
            string oldPath = context.Request.Path;
            string newPath = urlReWriter.GetRewrittenUrl(oldPath, context.Request.Url.Query);
            if (newPath != null && oldPath != newPath)
            {
                string qs = null;
                int index = newPath.IndexOf('?');
                if (index >= 0)
                {
                    qs = (index < (newPath.Length - 1)) ? newPath.Substring(index + 1) : string.Empty;
                    newPath = newPath.Substring(0, index);
                }
                urlReWriter.RewriteUrl(context, newPath, qs);
            }
        }
        private void SetCurrentUser()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                HttpContext.Current.User = UserManager.GetAnonymousUser();
            }
            else
            {
                try
                {
                    User user = UserManager.GetUser(new Guid(HttpContext.Current.User.Identity.Name));
                    if (user != null)
                    {
                        HttpContext.Current.User = user;
                    }
                    else
                    {
                        MemberManager.Logout();
                        HttpContext.Current.User = UserManager.GetAnonymousUser();
                    }
                }
                catch
                {
                    MemberManager.Logout();
                    HttpContext.Current.User = UserManager.GetAnonymousUser();
                }
            }
        }

        #endregion
    }
}