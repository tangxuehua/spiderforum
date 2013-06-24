using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class ControlPanelNavbar : UserControl
    {
        protected NoneStateRepeater tabRepeater;

        public string FileLocation
        {
            get
            {
                string path = Context.Server.MapPath(this.ResolveUrl(FileName));
                return path;
            }
        }
        public string FileName
        {
            get
            {
                Object state = ViewState["FileName"];
                if (state != null)
                {
                    return (String)state;
                }
                return "~/ControlPanelNavBar.xml";
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }
        public string ResourceFile
        {
            get
            {
                Object state = ViewState["ResourceFile"];
                if (state != null)
                {
                    return (String)state;
                }
                return "ControlPanelResources.xml";
            }
            set
            {
                ViewState["ResourceFile"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            tabRepeater.ItemDataBound += new RepeaterItemEventHandler(tabRepeater_ItemDataBound);
            BindData();
        }

        public string GetText(Tab t)
        {
            if (t.HasText)
            {
                return t.Text;
            }
            else
            {
                if (string.IsNullOrEmpty(t.ResourceFile))
                {
                    return ResourceManager.GetString(t.ResourceName, ResourceFile);
                }
                else
                {
                    return ResourceManager.GetString(t.ResourceName, t.ResourceFile);
                }
            }
        }
        public string FormatLink(Tab t)
        {
            string url = null;

            if (t.UrlName == null)
            {
                if (!t.HasHref)
                    return null;

                if (t.HasQueryString)
                    url = string.Format(t.Href.Replace('^', '&'), Context.Request.QueryString[t.QueryString]);
                else
                    url = t.Href;
            }
            else
            {
                object[] parameters = GetTotalParameters(t);
                if (parameters != null && parameters.Length > 0)
                {
                    url = FormatUrl(t.UrlName, parameters);
                }
                else
                {
                    url = FormatUrl(t.UrlName);
                }
            }

            return ResolveUrl(url);
        }
        public string FormatUrl(string name)
        {
            return UrlManager.Instance.FormatUrl(name);
        }
        public string FormatUrl(string name, params object[] parameters)
        {
            return UrlManager.Instance.FormatUrl(name, parameters);
        }

        private void BindData()
        {
            if (tabRepeater != null)
            {
                this.tabRepeater.DataSource = GetTabs().Tabs;
                this.tabRepeater.DataBind();
            }
        }
        private void tabRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Tab tab = e.Item.DataItem as Tab;
            NoneStateRepeater subTabRepeater = e.Item.FindControl("subTabRepeater") as NoneStateRepeater;
            if (tab.Children.Length > 0)
            {
                tab.Children[tab.Children.Length - 1].IsLastChild = true;
            }
            subTabRepeater.DataSource = tab.Children;
            subTabRepeater.DataBind();
        }
        private TabCollection GetTabs()
        {
            string path = FileLocation;

            if (path.StartsWith("/") || path.StartsWith("~"))
                path = Context.Server.MapPath(path);

            TabCollection tc = CacheManager.Get(path) as TabCollection;
            if (tc == null)
            {
                tc = (TabCollection)SerializeManager.ConvertFileToObject(path, typeof(TabCollection));
                CacheManager.Max(path, tc, new CacheDependency(path));
            }
            TabCollection filterTabs = new TabCollection();
            List<Tab> tabs = new List<Tab>();
            foreach (Tab tab in tc.Tabs)
            {
                if (tab.IsValid((User)HttpContext.Current.User))
                {
                    tabs.Add(tab);
                }
            }
            filterTabs.Tabs = tabs.ToArray();
            return filterTabs;
        }
        private object[] GetTotalParameters(Tab tab)
        {
            List<object> parameters = new List<object>();

            if (tab.HasParameters)
            {
                parameters.AddRange(tab.Parameters);
            }
            if (tab.HasParametersProvider)
            {
                Type providerType = Type.GetType(tab.ParametersProvider);
                IObjectsProvider objectsProvider = Activator.CreateInstance(providerType) as IObjectsProvider;
                if (objectsProvider != null)
                {
                    parameters.AddRange(objectsProvider.GetObjects());
                }
            }

            return parameters.ToArray();
        }
    }
}

