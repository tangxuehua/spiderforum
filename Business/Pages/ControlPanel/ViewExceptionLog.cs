using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class ViewExceptionLogPage : ForumBasePage
    {
        #region Private Members

        private ExceptionLog exceptionLog = null;

        #endregion

        #region Public Properties

        public ExceptionLog CurrentExceptionLog
        {
            get
            {
                return exceptionLog;
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnFirstLoad()
        {
            if (!ValidatePermission(PermissionType.ExceptionLogAdmin))
            {
                throw new Exception("您没有查看错误日志的权限！");
            }
        }

        #endregion

        #region Public Methods

        public override void GetRequests(List<RequestBinder> requestBinders)
        {
            requestBinders.Add(BinderBuilder.BuildGetBinder(this, new TRequest<ExceptionLog>(GetValue<int>(ParameterName.ExceptionLogId))));
        }
        public override void GetReplies(List<RequestBinder> requestBinders)
        {
            exceptionLog = requestBinders[0].Reply.Entity as ExceptionLog;
        }

        #endregion
    }
}