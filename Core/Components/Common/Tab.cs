using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Core
{
    [Serializable]
    public class Tab
    {
        #region Private Members

        private string _text;
        private string _href;
        private string _name;
        private string _target = "_self";
        private string _queryString;
        private string _permissions;
        private string _imagename = null;
        private string _resourcename = null;
        private string _resourcefile = null;
        private string _urlname = null;
        private string _parametersProvider = null;
        private string[] _urlparameters = null;
        private bool _enable = true;
        private Tab[] _children;
        private bool _isRoot = false;
        private string _filter = null;
        private bool _isModal = false;
        private string _modalWidth;
        private string _modalHeight;
        private string _modalCallback;
        private bool _isJavaScript = false;
        private string _javaScript;
        private bool isExpand = false;
        private bool isLastChild = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Property Text (string)
        /// </summary>
        [XmlAttribute("text")]
        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }

        [XmlAttribute("modalwidth")]
        public string ModalWidth
        {
            get { return this._modalWidth; }
            set { this._modalWidth = value; }
        }

        [XmlAttribute("modalheight")]
        public string ModalHeight
        {
            get { return this._modalHeight; }
            set { this._modalHeight = value; }
        }

        [XmlAttribute("modalcallback")]
        public string ModalCallback
        {
            get { return this._modalCallback; }
            set { this._modalCallback = value; }
        }

        [XmlAttribute("filter")]
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
            }
        }

        /// <summary>
        /// Property Href (string)
        /// </summary>
        [XmlAttribute("href")]
        public string Href
        {
            get { return this._href; }
            set { this._href = value; }
        }

        /// <summary>
        /// Property Target (string)
        /// </summary>
        [XmlAttribute("target")]
        public string Target
        {
            get { return this._target; }
            set { this._target = value; }
        }

        [XmlAttribute("javascript")]
        public string JavaScript
        {
            get { return this._javaScript; }
            set { this._javaScript = value; }
        }

        /// <summary>
        /// Property urlname (string)
        /// </summary>
        [XmlAttribute("urlname")]
        public string UrlName
        {
            get { return this._urlname; }
            set { this._urlname = value; }
        }

        /// <summary>
        /// Property imagename (string)
        /// </summary>
        [XmlAttribute("imagename")]
        public string ImageName
        {
            get { return this._imagename; }
            set { this._imagename = value; }
        }

        /// <summary>
        /// Property parameters (string[])
        /// </summary>
        [XmlAttribute("parameters")]
        public string[] Parameters
        {
            get { return this._urlparameters; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] parameters = value[0].Split(new char[] { ","[0], ";"[0], "|"[0] }, StringSplitOptions.None);
                    this._urlparameters = parameters;
                }
            }
        }

        /// <summary>
        /// ParametersProvider (string)
        /// </summary>
        [XmlAttribute("parametersProvider")]
        public string ParametersProvider
        {
            get { return this._parametersProvider; }
            set { this._parametersProvider = value; }
        }

        /// <summary>
        /// Property Name (string)
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// Property QueryString (string)
        /// </summary>
        [XmlAttribute("querystring")]
        public string QueryString
        {
            get { return this._queryString; }
            set { this._queryString = value; }
        }

        /// <summary>
        /// Property Roles (string)
        /// </summary>
        [XmlAttribute("permissions")]
        public string Permissions
        {
            get { return this._permissions; }
            set { this._permissions = value; }
        }

        /// <summary>
        /// Property ResourceName (string)
        /// </summary>
        [XmlAttribute("resourcename")]
        public string ResourceName
        {
            get { return this._resourcename; }
            set { this._resourcename = value; }
        }

        /// <summary>
        /// Property ResourceFile (string)
        /// </summary>
        [XmlAttribute("resourcefile")]
        public string ResourceFile
        {
            get { return this._resourcefile; }
            set { this._resourcefile = value; }
        }

        /// <summary>
        /// Property Enable (bool)
        /// </summary>
        [XmlAttribute("enabled")]
        public bool Enabled
        {
            get { return this._enable; }
            set { this._enable = value; }
        }

        /// <summary>
        /// Property Enable (bool)
        /// </summary>
        [XmlAttribute("isRoot")]
        public bool IsRoot
        {
            get { return this._isRoot; }
            set { this._isRoot = value; }
        }

        [XmlAttribute("isexpand")]
        public bool IsExpand
        {
            get { return this.isExpand; }
            set { this.isExpand = value; }
        }

        public bool IsLastChild
        {
            get { return this.isLastChild; }
            set { this.isLastChild = value; }
        }

        /// <summary>
        /// Property IsModal (bool)
        /// </summary>
        [XmlAttribute("ismodal")]
        public bool IsModal
        {
            get { return this._isModal; }
            set { this._isModal = value; }
        }

        [XmlAttribute("isjavascript")]
        public bool IsJavaScript
        {
            get { return this._isJavaScript; }
            set { this._isJavaScript = value; }
        }

        /// <summary>
        /// Property Children (Tab[])
        /// </summary>
        [XmlArray("SubTabs")]
        public Tab[] Children
        {
            get { return this._children; }
            set { this._children = value; }
        }

        #endregion

        #region Has Helpers

        public bool HasChildren
        {
            get { return this.Children != null && this.Children.Length > 0; }
        }

        public bool HasQueryString
        {
            get { return !string.IsNullOrEmpty(this.QueryString); }
        }

        public bool HasPermissions
        {
            get { return !string.IsNullOrEmpty(this.Permissions); }
        }

        public bool HasText
        {
            get { return !string.IsNullOrEmpty(this.Text); }
        }

        public bool HasHref
        {
            get { return !string.IsNullOrEmpty(this.Href); }
        }

        public bool HasParameters
        {
            get { return (this.Parameters != null); }
        }

        public bool HasParametersProvider
        {
            get { return (this.ParametersProvider != null); }
        }

        #endregion

        public bool IsValid(User currentUser)
        {
            if (this.Enabled)
            {
                if (this.HasPermissions)
                {
                    MergedPermission mergedPermission = currentUser.GetPermissions();
                    foreach (string permission in Permissions.Split(';'))
                    {
                        if (mergedPermission.ValidatePermission(Convert.ToInt64(permission, 16)))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class TabCollection
    {
        private Tab[] _tabs;

        [XmlArray("Tabs")]
        public Tab[] Tabs
        {
            get { return this._tabs; }
            set { this._tabs = value; }
        }
    }
}