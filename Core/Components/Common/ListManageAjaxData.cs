using System;
using System.Reflection;

namespace System.Web.Core
{
    public class ListManageAjaxData
    {
        private string listContent = string.Empty;
        private string pagingContent = string.Empty;

        public string ListContent
        {
            get
            {
                return listContent;
            }
            set
            {
                listContent = value;
            }
        }
        public string PagingContent
        {
            get
            {
                return pagingContent;
            }
            set
            {
                pagingContent = value;
            }
        }
    }
}