using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Core;

namespace Forum.Business
{
    public class EditAvatarPage : ForumBasePage
    {
        #region Protected Members

        protected FileUpload avatarUpload;
        protected IButton saveButton;
        protected IButton deleteButton;

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (saveButton != null)
            {
                saveButton.Click += new EventHandler(SaveButton_Click);
            }
            if (deleteButton != null)
            {
                deleteButton.Click += new EventHandler(DeleteButton_Click);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(avatarUpload.PostedFile.FileName))
            {
                UserManager.UpdateUserAvatar(CurrentUser, avatarUpload.PostedFile);
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            UserManager.DeleteUserAvatar(CurrentUser);
        }
    }
}