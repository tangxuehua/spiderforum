using System.Collections.Generic;

namespace System.Web.Core
{
    public class EntityListRequestMeteData
    {
        #region Private Members

        private ICondition condition = null;
        private Type requestType = null;
        private string tableName = null;
        private ReturnEntity returnEntity = null;

        #endregion

        #region Public Properties

        public ICondition Condition
        {
            get
            {
                return condition;
            }
            set
            {
                condition = value;
            }
        }
        public Type RequestType
        {
            get
            {
                return requestType;
            }
            set
            {
                requestType = value;
            }
        }
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
        public ReturnEntity ReturnEntity
        {
            get
            {
                return returnEntity;
            }
            set
            {
                returnEntity = value;
            }
        }

        #endregion
    }
}
