using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web.Core
{
    public class NavigationMenu : UserControl
    {
        private string _selected = null;
        private bool _reverse = false;
        protected NoneStateRepeater Menu;

        public string Selected
        {
            get
            {
                if (_selected == null)
                    _selected = Context.Items["SelectedNavigation"] as string;

                return _selected;
            }
            set
            {
                _selected = value;
            }
        }
        public bool Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            string cacheKey = this.GetType().ToString();

            ArrayList links = CacheManager.Get(cacheKey) as ArrayList;
            ArrayList filterLinks = new ArrayList();

            if (links == null)
            {
                links = UrlManager.Instance.Navigations;
                CacheManager.Insert(cacheKey, links, CacheManager.MinuteFactor * 15);
            }

            foreach (NavLink link in links)
            {
                //RoleList roleList = WebContext.Current.User.RoleList;
                //bool authorized = false;
                //foreach (Role role in roleList)
                //{
                //    if (link.Roles.ToLower().IndexOf(role.Name.Trim().ToLower()) >= 0)
                //    {
                //        authorized = true;
                //        break;
                //    }
                //}
                //if (authorized == false)
                //{
                //    continue;
                //}
                filterLinks.Add(link);
            }

            if (Reverse)
            {
                filterLinks = filterLinks.Clone() as ArrayList;
                filterLinks.Reverse();
            }

            Menu.DataSource = filterLinks;
            Menu.DataBind();
        }
    }
}