using System.Collections.Generic;

namespace System.Web.Core
{
    public class FieldCondition : IFieldCondition
    {
        #region Private Members

        private Type dataValidatorType = null;
        private string propertyPath = null;
        private string parameterName = "@" + string.Join("", Guid.NewGuid().ToString().Split(new char[] { '-' }));
        private object propertyValue = null;
        private string dbOperator = null;
        private IChildCondition childCondition = null;
        private List<ICondition> childConditions = new List<ICondition>();

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
        public IChildCondition ChildCondition
        {
            get
            {
                return this.childCondition;
            }
            set
            {
                this.childCondition = value;
            }
        }

        #endregion

        #region Constructors

        public FieldCondition()
        {
        }
        public FieldCondition(Type dataValidatorType, string propertyPath)
        {
            this.dataValidatorType = dataValidatorType;
            this.propertyPath = propertyPath;
            this.dbOperator = "=";
        }
        public FieldCondition(Type dataValidatorType, string propertyPath, string dbOperator)
        {
            this.dataValidatorType = dataValidatorType;
            this.propertyPath = propertyPath;
            this.dbOperator = dbOperator;
        }
        public FieldCondition(Type dataValidatorType, string propertyPath, object propertyValue, string dbOperator)
        {
            this.dataValidatorType = dataValidatorType;
            this.propertyPath = propertyPath;
            this.propertyValue = propertyValue;
            this.dbOperator = dbOperator;
        }

        #endregion

        #region IFieldCondition 成员

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
        public string ParameterName
        {
            get
            {
                return parameterName;
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
        public object PropertyValue
        {
            get
            {
                return propertyValue;
            }
            set
            {
                propertyValue = value;
            }
        }
        public object GetConditionValue(Request request)
        {
            if (PropertyValue == null && string.IsNullOrEmpty(PropertyPath))
            {
                throw new Exception("The PropertyPath and PropertyValue can not all be null.");
            }
            if (PropertyValue != null)
            {
                return PropertyValue;
            }
            return Configuration.Instance.GetPropertyPathValue(request, PropertyPath);
        }

        #endregion

        #region ICondition 成员

        public List<ICondition> ChildConditions
        {
            get
            {
                return childConditions;
            }
        }
        public bool IsValid(Request request)
        {
            if (DataValidatorType != null && ChildCondition != null)
            {
                IDataValidator validator = Activator.CreateInstance(DataValidatorType) as IDataValidator;
                if (validator != null)
                {
                    return validator.Validate(GetConditionValue(request)) && ChildCondition.IsValid(request);
                }
                return false;
            }
            else if (DataValidatorType != null)
            {
                IDataValidator validator = Activator.CreateInstance(DataValidatorType) as IDataValidator;
                if (validator != null)
                {
                    return validator.Validate(GetConditionValue(request));
                }
                return false;
            }
            else if (ChildCondition != null)
            {
                return ChildCondition.IsValid(request);
            }
            return false;
        }
        public string GetConditionExpression(Request request)
        {
            Type entityType = request.Entity.GetType();
            if (DbOperator == "In")
            {
                object value = GetConditionValue(request);
                if (value != null)
                {
                    return string.Format(" t.{0} {1} {2}", Configuration.Instance.GetDBFieldName(entityType, GetEntityPropertyPath(entityType, this)), DbOperator, value.ToString());
                }
                else
                {
                    throw new Exception("Condition value can not be null if the db operator is 'In'.");
                }
            }
            return string.Format(" t.{0} {1} {2}", Configuration.Instance.GetDBFieldName(entityType, GetEntityPropertyPath(entityType, this)), DbOperator, ParameterName);
        }
        public string GetConditionExpressionWithoutAliasName(Request request)
        {
            Type entityType = request.Entity.GetType();
            if (DbOperator == "In")
            {
                object value = GetConditionValue(request);
                if (value != null)
                {
                    return string.Format(" {0} {1} {2}", Configuration.Instance.GetDBFieldName(entityType, GetEntityPropertyPath(entityType, this)), DbOperator, value.ToString());
                }
                else
                {
                    throw new Exception("Condition value can not be null if the db operator is 'In'.");
                }
            }
            return string.Format(" {0} {1} {2}", Configuration.Instance.GetDBFieldName(entityType, GetEntityPropertyPath(entityType, this)), DbOperator, ParameterName);
        }
        public bool HasInOperatorCondition
        {
            get
            {
                return DbOperator == "In";
            }
        }

        #endregion

        #region Private Methods

        private string GetEntityPropertyPath(Type entityType, IFieldCondition fieldCondition)
        {
            string entityPropertyPath = fieldCondition.PropertyPath;
            if (entityPropertyPath.StartsWith("Data."))
            {
                entityPropertyPath = entityPropertyPath.Substring(5);
            }
            return entityPropertyPath;
        }

        #endregion
    }
}
