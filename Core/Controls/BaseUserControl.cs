using System;
using System.Collections.Generic;
using System.Web.UI;

namespace System.Web.Core
{
    public class BaseUserControl : UserControl, IRequestBuilder, IReplyConverter
    {
        #region Private Members

        private Entity currentEntity;

        #endregion

        #region Constructors

        public BaseUserControl()
        {
            BasePage page = (BasePage)HttpContext.Current.CurrentHandler;
            page.RegisterRequestBuilder(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// This property indicates the current edit or view entity of the current user control.
        /// </summary>
        public Entity CurrentEntity
        {
            get
            {
                return currentEntity;
            }
            set
            {
                currentEntity = value;
            }
        }
        /// <summary>
        /// This property indicates whether the control is load for the first time.
        /// </summary>
        public bool IsFirstLoad
        {
            get
            {
                return !Page.IsPostBack && !Page.IsCallback && !AjaxManager.IsCallBack;
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AjaxManager.Register(this, ClientID);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsFirstLoad)
            {
                OnFirstLoad();
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
    }
}