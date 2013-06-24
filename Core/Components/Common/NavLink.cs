namespace System.Web.Core
{
    public class NavLink
    {
        private string name = null;
        private string resourceName = null;
        private string text = null;
        private string navigateUrl = null;
        private string roles = null;
        private string target = null;
        private string cssClass = null;
        private string navType = null;

        public NavLink(string name, string resourceName, string text, string navigateUrl, string target, string cssClass, string roles, string navType)
        {
            this.name = name;
            this.resourceName = resourceName;
            this.text = text;
            this.navigateUrl = navigateUrl;
            this.roles = roles;
            this.target = target;
            this.navType = navType;
            this.cssClass = cssClass;
        }

        public string Text
        {
            get
            {
                if(resourceName != null)
                    return ResourceManager.GetString(resourceName);
                else
                    return text;
            }
        }

        public string ResourceName
        {
            get
            {
                return resourceName;
            }
        }

        public string NavigateUrl
        {
            get
            {
                return navigateUrl;
            }
        }

        public string Target
        {
            get
            {
                return target;
            }
        }

        public string CssClass
        {
            get
            {
                return cssClass;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Roles
        {
            get
            {
                return roles;
            }
        }

        public string NavType
        {
            get
            {
                return navType;
            }
        }
    }
}
