using System;

namespace System.Web.Core
{
    public class OrderField
    {
        private string propertyPath = string.Empty;
        private bool isAscending = true;

        public string PropertyPath
        {
            get
            {
                return propertyPath;
            }
            set
            {
                propertyPath = value;
            }
        }
        public bool IsAscending
        {
            get
            {
                return isAscending;
            }
            set
            {
                isAscending = value;
            }
        }

        public OrderField()
        {
        }
        public OrderField(string propertyPath, bool isAscending)
        {
            this.propertyPath = propertyPath;
            this.isAscending = isAscending;
        }
    }
}