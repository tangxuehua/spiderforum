using System;
using System.Web.UI;
using System.Configuration;
using System.Collections.Generic;

namespace System.Web.Core
{
    public class BasePage : Page, IRequestBuilder, IReplyConverter
    {
        #region Private Members

        private List<IRequestBuilder> requestBuilders = new List<IRequestBuilder>();

        #endregion

        #region Constructors

        public BasePage()
        {
            RegisterRequestBuilder(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// This property indicates whether the page is load for the first time.
        /// </summary>
        public bool IsFirstLoad
        {
            get
            {
                return !IsPostBack && !IsCallback && !AjaxManager.IsCallBack;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This function allow user to register the request builder into the page.
        /// </summary>
        /// <param name="requestBuilder"></param>
        public void RegisterRequestBuilder(IRequestBuilder requestBuilder)
        {
            if (requestBuilder != null && !requestBuilders.Contains(requestBuilder))
            {
                requestBuilders.Add(requestBuilder);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnLoad(e);
            AjaxManager.Register(this, ClientID);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsFirstLoad)
            {
                OnFirstLoad();
                ProcessRequests();
                if (NeedReply != null)
                {
                    NeedReply(this);
                }
            }
        }
        protected virtual void OnFirstLoad()
        {
        }
        protected static TValue GetValue<TValue>(object name)
        {
            return UrlManager.Instance.GetParameterValue<TValue>(name);
        }

        #endregion

        #region IRequestBuilder 成员

        /// <summary>
        /// This function used to add all the requests of the current user control.
        /// This function will only be called when the page was first load.
        /// </summary>
        public virtual void GetRequests(List<RequestBinder> requestBinders)
        {
        }

        #endregion

        #region IReplyConverter 成员

        public event NeedReplyEventHandler NeedReply;
        public virtual void GetReplies(List<RequestBinder> requestBinders)
        {
        }

        #endregion

        #region Private Methods

        private void ProcessRequests()
        {
            Engine engine = new Engine();
            foreach (IRequestBuilder requestBuilder in requestBuilders)
            {
                requestBuilder.GetRequests(engine.EntityRequests);
            }
            engine.ExecuteRequestList();
        }

        #endregion
    }
}