using System.Collections.Generic;

namespace System.Web.Core
{
    public class Condition
    {
        #region Private Members

        private Type dataValidatorType = null;
        private object value = null;
        private string dbOperator = null;

        #endregion

        #region Constructors

        public Condition()
        {
        }
        public Condition(Type dataValidatorType, string dbOperator, object value)
        {
            this.dataValidatorType = dataValidatorType;
            this.dbOperator = dbOperator;
            this.value = value;
        }

        #endregion

        #region Public Properties

        public Type DataValidatorType
        {
            get
            {
                return dataValidatorType;
            }
            set
            {
                dataValidatorType = value;
            }
        }
        public string DbOperator
        {
            get
            {
                return dbOperator;
            }
            set
            {
                dbOperator = value;
            }
        }
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        #endregion
    }
}
