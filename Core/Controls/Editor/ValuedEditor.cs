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

namespace System.Web.Core
{
    public class ValuedEditor : UserControl, IValuedControl
    {
        protected ITextEditor editor;
        private Unit width;
        private Unit height;

        public string Value
        {
            get
            {
                EnsureChildControls();
                return editor.Text;
            }
            set
            {
                EnsureChildControls();
                editor.Text = value;
            }
        }
        public Unit Height
        {
            set
            {
                height = value;
            }
            get
            {
                return height;
            }
        }
        public Unit Width
        {
            set
            {
                width = value;
            }
            get
            {
                return width;
            }
        }
        public string EditorUniqueID
        {
            get
            {
                return ((Control)editor).UniqueID;
            }
        }
        public string EditorClientID
        {
            get
            {
                EnsureChildControls();
                return ((Control)editor).ClientID;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack && !AjaxManager.IsCallBack)
            {
                editor.Width = width;
                editor.Height = height;
            }
        }
    }
}