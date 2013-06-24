using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Core;
using System.Drawing;
using System.IO;

namespace System.Web.Core
{
    public class UserAvatar : PlaceHolder
    {
        #region Private Members

        private int defaultWidth = 80;
        private int defaultHeight = 100;

        #endregion

        #region Constructors

        public UserAvatar()
        {
            this.EnableViewState = true;
        }

        #endregion

        #region Public Properties

        public int UserId
        {
            get
            {
                object state = ViewState["UserId"];
                if (state != null)
                {
                    return (int)state;
                }
                return 0;
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }
        public string AvatarFileName
        {
            get
            {
                object state = ViewState["AvatarFileName"];
                if (state != null)
                {
                    return state as string;
                }
                return string.Empty;
            }
            set
            {
                ViewState["AvatarFileName"] = value;
            }
        }
        public string AvatarUrl
        {
            get
            {
                object state = ViewState["AvatarUrl"];
                if (state != null)
                {
                    return state as string;
                }
                return "userAvatar.ashx";
            }
            set
            {
                ViewState["AvatarUrl"] = value;
            }
        }
        public int Border
        {
            get
            {
                object state = ViewState["Border"];
                if (state != null)
                {
                    return (int)state;
                }
                return 0;
            }
            set
            {
                ViewState["Border"] = value;
            }
        }
        public int Height
        {
            get
            {
                object state = ViewState["Height"];
                if (state != null)
                {
                    return (int)state;
                }
                return 0;
            }
            set
            {
                ViewState["Height"] = value;
            }
        }
        public int Width
        {
            get
            {
                object state = ViewState["Width"];
                if (state != null)
                {
                    return (int)state;
                }
                return 0;
            }
            set
            {
                ViewState["Width"] = value;
            }
        }

        #endregion

        #region Override Methods

        protected override void Render(HtmlTextWriter writer)
        {
            HtmlImage image = new HtmlImage();

            int avatarWidth = defaultWidth;
            int avatarHeight = defaultHeight;
            if (Width > 0 && Height > 0)
            {
                avatarWidth = Width;
                avatarHeight = Height;
            }

            string parameters = string.Format("?{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                ParameterName.PictureWidth, avatarWidth,
                ParameterName.PictureHeight, avatarHeight,
                ParameterName.UserId, UserId,
                ParameterName.FileName, AvatarFileName
                );

            image.Src = Globals.ApplicationPath + "/" + AvatarUrl.TrimStart('/') + parameters;
            image.Border = Border;
            image.RenderControl(writer);
        }

        #endregion
    }
}