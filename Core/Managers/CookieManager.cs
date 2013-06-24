using System;
using System.Web;
using System.Web.Security;

namespace System.Web.Core
{
    public class CookieManager
    {
        private CookieManager()
        {
        }

        public static string GetCookieValue(string cookieName)
        {
            return GetCookieValue(cookieName, null);
        }
        public static string GetCookieValue(string cookieName, string defaultValue)
        {
            if (!IsCookieExist(HttpContext.Current.Request.Cookies, cookieName))
            {
                return defaultValue;
            }
            return HttpContext.Current.Request.Cookies[cookieName].Value;
        }
        public static void SaveCookie(string cookieName, string cookieValue, int expireDays)
        {
            HttpCookie cookie = null;
            if (!IsCookieExist(HttpContext.Current.Response.Cookies, cookieName))
            {
                cookie = new HttpCookie(cookieName);
                cookie.Value = cookieValue == null ? "" : cookieValue;
                AddCookieToResponse(cookie, expireDays);
            }
            else
            {
                cookie = HttpContext.Current.Response.Cookies.Get(cookieName);
                cookie.Value = cookieValue == null ? "" : cookieValue;
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Expires = DateTime.Now.AddDays(expireDays);
            }
        }
        public static void AddCookieToResponse(HttpCookie cookie, int expireDays)
        {
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Expires = DateTime.Now.AddDays(expireDays);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void AddCookieToResponse(HttpCookie cookie)
        {
            cookie.Domain = FormsAuthentication.CookieDomain;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void DeleteCookie(string cookieName)
        {
            if (IsCookieExist(HttpContext.Current.Request.Cookies, cookieName))
            {
                if (!IsCookieExist(HttpContext.Current.Response.Cookies, cookieName))
                {
                    AddCookieToResponse(new HttpCookie(cookieName), -1);
                }
                else
                {
                    HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }
        public static bool IsCookieExist(HttpCookieCollection cookieCollection, string cookieName)
        {
            foreach (string cookieKey in cookieCollection.Keys)
            {
                if (cookieKey == cookieName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}