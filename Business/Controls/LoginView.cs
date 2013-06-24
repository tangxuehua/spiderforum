using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace Forum.Business
{
    [ParseChildren(true), PersistChildren(false)]
    public class LoginView : Control, INamingContainer
    {
        private ITemplate anonymousTemplate;
        private ITemplate loggedInTemplate;

        protected override void CreateChildControls()
        {
            Controls.Clear();

            ITemplate template = null;

            if (((ForumUser)HttpContext.Current.User).IsAnonymous)
            {
                template = AnonymousTemplate;
            }
            else
            {
                template = LoggedInTemplate;
            }

            if (template != null)
            {
                Control cntrl = new Control();
                template.InstantiateIn(cntrl);
                Controls.Add(cntrl);
            }
        }

        [Browsable(false), DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(LoginView))]
        public ITemplate AnonymousTemplate
        {
            get
            {
                return anonymousTemplate;
            }
            set
            {
                anonymousTemplate = value;
            }
        }

        [Browsable(false), DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(LoginView))]
        public ITemplate LoggedInTemplate
        {
            get
            {
                return loggedInTemplate;
            }
            set
            {
                loggedInTemplate = value;
            }
        }
    }
}


