using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Forum.Business 
{
    public class EditProfilePage : ForumBasePage
    {
        #region Protected Members

        protected ValuedTextBox nickNameValuedTextBox;

        #endregion

        #region OnInitialize

        protected override void OnFirstLoad()
        {
            nickNameValuedTextBox.Value = CurrentUser.NickName.Value;
        }

        #endregion

        #region Ajax Methods

        [AjaxMethod]
        public void SaveProfile(string nickName)
        {
            CurrentUser.NickName.Value = nickName;

            ThreadRequest threadRequest1 = new ThreadRequest();
            threadRequest1.Data.AuthorId.Value = CurrentUser.EntityId.Value;
            threadRequest1.UpdatePropertyEntryList.Add(new UpdatePropertyEntry("Author", nickName));

            ThreadRequest threadRequest2 = new ThreadRequest();
            threadRequest2.Data.MostRecentReplierId.Value = CurrentUser.EntityId.Value;
            threadRequest2.UpdatePropertyEntryList.Add(new UpdatePropertyEntry("MostRecentReplierName", nickName));

            Engine.Executes(BinderBuilder.BuildUpdateBinder(CurrentUser), BinderBuilder.BuildUpdateListBinder(threadRequest1), BinderBuilder.BuildUpdateListBinder(threadRequest2));
        }

        #endregion
    }
}