using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Core;
using System.Web;

namespace System.Web.Core
{
    public class EntityImage : Control
    {
        #region Private Members

        private int width = 100;
        private int height = 100;
        private AttachmentEntity attachment;
        
        #endregion

        #region Public Properties

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        public AttachmentEntity Attachment
        {
            get
            {
                return attachment;
            }
            set
            {
                attachment = value;
            }
        }

        #endregion

        #region Override Methods

        protected override void Render(HtmlTextWriter writer)
        {
            if (Attachment == null)
            {
                Attachment = GetAttachmentFromParent();
            }
            if (Attachment == null)
            {
                return;
            }

            HtmlImage image = new HtmlImage();

            string imageFileUrl = Globals.GetFileStorageUrl(attachment.AttachmentFileName.Value);
            int imageWidth = width;
            int imageHeight = height;

            if (Width > 0 && Height > 0)
            {
                imageWidth = Width;
                imageHeight = Height;
            }

            image.Src = Globals.ApplicationPath + @"/image.ashx" + string.Format("?{0}={1}&{2}={3}&{4}={5}", ParameterName.PictureWidth, imageWidth, ParameterName.PictureHeight, imageHeight, ParameterName.FileUrl, imageFileUrl);

            image.RenderControl(writer);
        }

        #endregion

        #region Private Methods

        private AttachmentEntity GetAttachmentFromParent()
        {
            BaseUserControl baseUserControl = Globals.FindParent<BaseUserControl>(this);
            if (baseUserControl != null)
            {
                return baseUserControl.CurrentEntity as AttachmentEntity;
            }
            return null;
        }

        #endregion
    }
}
