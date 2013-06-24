using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;
using System.Text;

namespace System.Web.Core
{
    public class SqlEntityProvider : EntityProvider
    {
        #region Private Consts

        private const int commandTimeout = 120;
        private const string executeSQL = @"EXECUTE sp_executesql {0}";
        private const string updateEntityListSQL = @"N'UPDATE [{0}] SET {1} WHERE{2}'{3};";
        private const string deleteEntityListSQL = @"N'DELETE FROM [{0}] WHERE{1}'{2};";
        private const string selectAllEntitiesSQL = @"N'SELECT {1} FROM [{0}] t WHERE 1 = 1{2}{4}'{3}";
        private const string selectEntityListSQL = @"
                        SET NOCOUNT ON

                        --Get total records count
                        DECLARE @sql NVARCHAR(4000)

                        SET @sql= N'SELECT @TotalRecords = COUNT(*) FROM ({0}) a'
                        EXECUTE sp_executesql @sql, N'{2}@TotalRecords INT OUTPUT', {3}@TotalRecords OUTPUT

                        -- Get the records of the current page
                        SET @sql = N'SELECT TOP ' + N'{5}' + N'{6}' + N' FROM (SELECT TOP ' + N'{5}' + N'{6}' + N' FROM ({1}) t ' + N'{7}' + N') t ' + N'{8}'
                        EXECUTE sp_executesql @sql{4}

                        SET NOCOUNT OFF
                    ";

        #endregion

        #region Private Members

        private string connectionString;

        #endregion

        #region Override Methods

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (connectionStringName == null || connectionStringName.Length < 1)
            {
                throw new ProviderException("Connection name not specified.");
            }
            connectionString = ConfigurationManager.AppSettings[connectionStringName];
            if (connectionString == null || connectionString.Length < 1)
            {
                throw new ProviderException("Connection string not found.");
            }
        }
        public override void ProcessRequest(RequestBinder requestBinder)
        {
            ProcessRequest(CreateConnection(), null, requestBinder);
        }
        public override void ProcessRequests(List<RequestBinder> requestBinders)
        {
            SqlConnection connection = CreateConnection();
            SqlTransaction transaction = connection.BeginTransaction();
            bool isCommittingTransaction = false;
            try
            {
                foreach (RequestBinder binder in requestBinders)
                {
                    if (requestBinders.IndexOf(binder) > 0)
                    {
                        UpdateRequestDependencies(binder.Request);
                    }
                    ProcessRequest(connection, transaction, binder);
                }
                isCommittingTransaction = true;
                transaction.Commit();
            }
            catch
            {
                if (!isCommittingTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
                CloseConnection(connection);
            }
        }

        #endregion

        #region Private Methods

        private void ProcessRequest(SqlConnection connection, SqlTransaction transaction, RequestBinder requestBinder)
        {
            if (!string.IsNullOrEmpty(requestBinder.Request.CommandIdent))
            {
                ExecuteCustomizeCommand(connection, transaction, Configuration.Instance.GetCommand(requestBinder.Request.CommandIdent), requestBinder.Request, requestBinder.Reply, requestBinder.Request.OperationType);
                return;
            }
            
            Type entityType = requestBinder.Request.Entity.GetType();
            Table table = null;
            EntityMapping entityMapping = null;
            OperationType operationType = requestBinder.Request.OperationType;
            object sourceObject = null;
            object replyObject = null;

            if (operationType != OperationType.Create &&
                operationType != OperationType.Update &&
                operationType != OperationType.Delete &&
                operationType != OperationType.Get &&
                operationType != OperationType.GetList &&
                operationType != OperationType.UpdateList &&
                operationType != OperationType.DeleteList)
            {
                return;
            }

            switch (operationType)
            {
                case OperationType.Create:
                    table = Configuration.Instance.GetTable(entityType);
                    entityMapping = Configuration.Instance.GetEntityMapping(entityType);
                    sourceObject = requestBinder.Request.Entity;
                    replyObject = requestBinder.Request.Entity;
                    break;
                case OperationType.Update:
                    table = Configuration.Instance.GetTable(entityType);
                    entityMapping = Configuration.Instance.GetEntityMapping(entityType);
                    sourceObject = requestBinder.Request.Entity;
                    break;
                case OperationType.Delete:
                    table = Configuration.Instance.GetTable(entityType);
                    entityMapping = Configuration.Instance.GetEntityMapping(entityType);
                    sourceObject = requestBinder.Request;
                    break;
                case OperationType.Get:
                    table = Configuration.Instance.GetTable(entityType);
                    entityMapping = Configuration.Instance.GetEntityMapping(entityType);
                    sourceObject = requestBinder.Request;
                    replyObject = requestBinder.Reply;
                    break;
                case OperationType.GetList:
                    sourceObject = requestBinder.Request;
                    replyObject = requestBinder.Reply;
                    break;
                case OperationType.UpdateList:
                    sourceObject = requestBinder.Request;
                    table = Configuration.Instance.GetTable(entityType);
                    break;
                case OperationType.DeleteList:
                    sourceObject = requestBinder.Request;
                    table = Configuration.Instance.GetTable(entityType);
                    break;
            }

            ExecuteCRUDCommand(connection, transaction, table, entityMapping, sourceObject, replyObject, operationType);
        }

        private void ExecuteCustomizeCommand(SqlConnection connection, SqlTransaction transaction, Command command, object sourceObject, object replyObject, OperationType operationType)
        {
            if (operationType == OperationType.Get || operationType == OperationType.GetList)
            {
                ProcessGetEntityOrEntityListCustomizeCommand(connection, transaction, command, (Request)sourceObject, (Reply)replyObject);
                return;
            }
            try
            {
                SqlCommand sqlCommand = new SqlCommand(command.CommandName, connection);

                sqlCommand.Transaction = transaction;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = commandTimeout;

                SetCommandParameters(command, sqlCommand);

                SqlParameter returnParameter = CreateReturnParameter();
                sqlCommand.Parameters.Add(returnParameter);

                SetInputParameterValues(command, sqlCommand, sourceObject);

                sqlCommand.ExecuteNonQuery();

                if (command.OutputParameters.Count > 0)
                {
                    SetOutputParameterValues(command.OutputParameters, sqlCommand, replyObject);
                }
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection(connection);
                }
            }
        }
        private void ExecuteCRUDCommand(SqlConnection connection, SqlTransaction transaction, Table table, EntityMapping entityMapping, object sourceObject, object replyObject, OperationType operationType)
        {
            if (operationType == OperationType.GetList)
            {
                Request request = sourceObject as Request;
                if (request.GetAll)
                {
                    ProcessGetAllEntitiesSQL(connection, transaction, request, (Reply)replyObject);
                    return;
                }
                ProcessGetPagedEntityListSQL(connection, transaction, request, (Reply)replyObject);
                return;
            }

            try
            {
                SqlCommand sqlCommand = new SqlCommand();

                sqlCommand.Connection = connection;
                sqlCommand.CommandTimeout = commandTimeout;
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandType = CommandType.Text;

                if (operationType == OperationType.Create || operationType == OperationType.Update || operationType == OperationType.Delete || operationType == OperationType.Get)
                {
                    sqlCommand.CommandText = GetCRUDCommandText(table, operationType);
                    SetCommandParameters(table, sqlCommand, operationType);
                    SetInputParameterValues(entityMapping, sqlCommand, sourceObject, operationType);
                }

                if (operationType == OperationType.Create || operationType == OperationType.Update || operationType == OperationType.Delete)
                {
                    sqlCommand.ExecuteNonQuery();
                    if (operationType == OperationType.Create)
                    {
                        SqlParameter sqlParameter = sqlCommand.Parameters["@EntityId"];
                        if (sqlParameter.Value != DBNull.Value)
                        {
                            UpdateParentObject(replyObject, "EntityId", sqlParameter.Value);
                        }
                    }
                }
                else if (operationType == OperationType.Get)
                {
                    IDataReader reader = sqlCommand.ExecuteReader();
                    ReturnEntity returnEntity = entityMapping.GetDefaultReturnEntity();
                    returnEntity.EntityReturnMode = EntityReturnMode.Single;
                    ProcessReturnEntity((Request)sourceObject, replyObject, "Entity", returnEntity, reader);
                    reader.Close();
                }
                else if (operationType == OperationType.DeleteList)
                {
                    Request request = sourceObject as Request;
                    ICondition condition = GetCondition(request);
                    string parametersFormat = GetParametersFormat(request, condition, sqlCommand);
                    string conditionExpression = string.Empty;

                    if (condition != null && condition.IsValid(request))
                    {
                        conditionExpression = condition.GetConditionExpressionWithoutAliasName(request);
                    }
                    if (string.IsNullOrEmpty(conditionExpression) && !condition.HasInOperatorCondition)
                    {
                        throw new Exception("You must specify at least one condition when deleting entities.");
                    }
                    sqlCommand.CommandText = string.Format(executeSQL, string.Format(deleteEntityListSQL, table.Name, conditionExpression, parametersFormat));
                    sqlCommand.ExecuteNonQuery();
                }
                else if (operationType == OperationType.UpdateList)
                {
                    Request request = sourceObject as Request;
                    ICondition condition = GetCondition(request);
                    string conditionExpression = string.Empty;
                    string updatePart = string.Empty;
                    string parametersFormat = string.Empty;

                    if (request.UpdatePropertyEntryList.Count == 0)
                    {
                        throw new Exception("You must specify at least one update field when updating entities.");
                    }

                    GetUpdatePartAndParameterFormat(request, condition, sqlCommand, ref updatePart, ref parametersFormat);

                    if (condition != null && condition.IsValid(request))
                    {
                        conditionExpression = condition.GetConditionExpressionWithoutAliasName(request);
                    }
                    if (string.IsNullOrEmpty(conditionExpression) && !condition.HasInOperatorCondition)
                    {
                        throw new Exception("You must specify at least one condition when updating entities.");
                    }
                    sqlCommand.CommandText = string.Format(executeSQL, string.Format(updateEntityListSQL, table.Name, updatePart, conditionExpression, parametersFormat));
                    sqlCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection(connection);
                }
            }
        }

        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        private void CloseConnection(SqlConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
        private void UpdateRequestDependencies(Request request)
        {
            foreach (RequestDependency dependency in request.RequestDependencies)
            {
                PropertyInfo targetProperty = dependency.TargetObject.GetType().GetProperty(dependency.TargetPropertyName);
                PropertyInfo sourceProperty = dependency.SourceObject.GetType().GetProperty(dependency.SourcePropertyName);
                object sourceValue = sourceProperty.GetValue(dependency.SourceObject, null);
                Property targetValue = Activator.CreateInstance(targetProperty.PropertyType) as Property;

                if (sourceValue != null && typeof(Property).IsAssignableFrom(sourceValue.GetType()))
                {
                    sourceValue = ((Property)sourceValue).ObjectValue;
                }
                if (targetValue != null)
                {
                    targetValue.ObjectValue = sourceValue;
                    targetProperty.SetValue(dependency.TargetObject, targetValue, null);
                }
                else
                {
                    targetProperty.SetValue(dependency.TargetObject, sourceValue, null);
                }
            }
        }
        private string GetCRUDCommandText(Table table, OperationType operationType)
        {
            if (operationType == OperationType.Create)
            {
                return table.CreateCommandText;
            }
            else if (operationType == OperationType.Update)
            {
                return table.UpdateCommandText;
            }
            else if (operationType == OperationType.Delete)
            {
                return table.DeleteCommandText;
            }
            else if (operationType == OperationType.Get)
            {
                return table.RetrieveCommandText;
            }
            return null;
        }
        private SqlCommand GetSqlCommand(SqlConnection connection, Command command, Request entityRequest)
        {
            SqlCommand sqlCommand = new SqlCommand(command.CommandName, connection);

            sqlCommand.CommandType = CommandType.StoredProcedure;
            SetCommandParameters(command, sqlCommand);
            SetInputParameterValues(command, sqlCommand, entityRequest);

            return sqlCommand;
        }
        private void SetCommandParameters(Table table, SqlCommand sqlCommand, OperationType operationType)
        {
            if (sqlCommand == null || table == null)
            {
                return;
            }

            SqlParameterCollection parameters = sqlCommand.Parameters;
            SqlParameter parameter;

            if (operationType == OperationType.Create || operationType == OperationType.Update)
            {
                foreach (TableField field in table.Fields)
                {
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@" + field.Name;
                    parameter.Direction = field.Name == "EntityId" && operationType == OperationType.Create ? ParameterDirection.Output : ParameterDirection.Input;
                    parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), field.TypeEnum);
                    sqlCommand.Parameters.Add(parameter);
                }
            }
            else if (operationType == OperationType.Get || operationType == OperationType.Delete)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = "@EntityId";
                parameter.Direction = ParameterDirection.Input;
                parameter.SqlDbType = SqlDbType.Int;
                sqlCommand.Parameters.Add(parameter);
            }
        }
        private void SetCommandParameters(Command command, SqlCommand sqlCommand)
        {
            if (sqlCommand == null || command == null)
            {
                return;
            }

            SqlParameterCollection parameters = sqlCommand.Parameters;
            SqlParameter parameter;

            foreach (CommandParameter commandParameter in command.LeafParameters)
            {
                parameter = new SqlParameter();

                parameter.ParameterName = commandParameter.ParameterName;
                parameter.Direction = commandParameter.RealParameterDirection;
                parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), commandParameter.DbTypeHint);

                sqlCommand.Parameters.Add(parameter);
            }
        }
        private void SetInputParameterValues(EntityMapping entityMapping, SqlCommand sqlCommand, object targetObject, OperationType operationType)
        {
            if (sqlCommand == null || targetObject == null)
            {
                return;
            }

            if (operationType == OperationType.Create || operationType == OperationType.Update)
            {
                object parameterValue = null;
                foreach (PropertyNode propertyNode in entityMapping.LeafPropertyNodes)
                {
                    if (propertyNode.PropertyPath == "EntityId" && operationType == OperationType.Create)
                    {
                        continue;
                    }
                    parameterValue = Configuration.Instance.GetPropertyPathValue(targetObject, propertyNode.PropertyPath);
                    if (parameterValue == null)
                    {
                        sqlCommand.Parameters["@" + propertyNode.FieldName].Value = DBNull.Value;
                    }
                    else
                    {
                        sqlCommand.Parameters["@" + propertyNode.FieldName].Value = parameterValue;
                    }
                }
            }
            else if (operationType == OperationType.Get || operationType == OperationType.Delete)
            {
                sqlCommand.Parameters["@EntityId"].Value = Configuration.Instance.GetPropertyPathValue(targetObject, "EntityId");
            }
        }
        private void SetInputParameterValues(Command command, SqlCommand sqlCommand, object targetObject)
        {
            object parameterValue = null;
            foreach (CommandParameter commandParameter in command.InputParameters)
            {
                //TODO, consider to use the data validator to check parameter value.
                parameterValue = Configuration.Instance.GetPropertyPathValue(targetObject, commandParameter.PropertyPath);
                if (parameterValue == null)
                {
                    sqlCommand.Parameters[commandParameter.ParameterName].Value = DBNull.Value;
                }
                else
                {
                    sqlCommand.Parameters[commandParameter.ParameterName].Value = parameterValue;
                }
            }
        }
        private void SetOutputParameterValues(List<CommandParameter> commandParemeters, SqlCommand sqlCommand, object parentObject)
        {
            SqlParameter sqlParameter;
            Entity propertyValue;
            foreach (CommandParameter commandParameter in commandParemeters)
            {
                if (commandParameter.Childs.Count > 0)
                {
                    propertyValue = CreateEntity(parentObject, commandParameter.PropertyName);
                    UpdateParentObject(parentObject, commandParameter.PropertyName, propertyValue);
                    SetOutputParameterValues(commandParameter.Childs, sqlCommand, propertyValue);
                }
                else
                {
                    sqlParameter = sqlCommand.Parameters[commandParameter.ParameterName];
                    if (sqlParameter.Value != DBNull.Value)
                    {
                        UpdateParentObject(parentObject, commandParameter.PropertyName, sqlParameter.Value);
                    }
                }
            }
        }
        private void PopulateEntity(Type type, IDataReader reader, ReturnEntity returnEntity)
        {
            Entity entity = Activator.CreateInstance(type) as Entity;
            foreach (PropertyNode propertyNode in returnEntity.PropertyNodes)
            {
                PopulateEntity(entity, propertyNode, reader);
            }
            returnEntity.Entity = entity;
        }
        private void PopulateEntity(Entity parentEntity, PropertyNode currentPropertyNode, IDataReader reader)
        {
            Entity currentEntity = null;
            if (currentPropertyNode.ChildNodes.Count > 0)
            {
                currentEntity = CreateEntity(parentEntity, currentPropertyNode.PropertyName);
                UpdateParentObject(parentEntity, currentPropertyNode.PropertyName, currentEntity);
                foreach (PropertyNode childNode in currentPropertyNode.ChildNodes)
                {
                    PopulateEntity(currentEntity, childNode, reader);
                }
            }
            else
            {
                UpdateParentObject(parentEntity, currentPropertyNode.PropertyName, reader[currentPropertyNode.FieldName]);
            }
        }
        private void UpdateParentObject(object parentObject, string propertyInParentObject, object propertyValue)
        {
            PropertyInfo propertyInfo = null;
            Property property = null;

            if (parentObject != null && !string.IsNullOrEmpty(propertyInParentObject) && propertyValue != null)
            {
                propertyInfo = parentObject.GetType().GetProperty(propertyInParentObject);
                property = Activator.CreateInstance(propertyInfo.PropertyType) as Property;
                if (property != null)
                {
                    property.ObjectValue = propertyValue;
                    propertyInfo.SetValue(parentObject, property, null);
                }
                else
                {
                    propertyInfo.SetValue(parentObject, propertyValue, null);
                }
            }
        }
        private Entity CreateEntity(object parentObject, string propertyName)
        {
            Type propertyType = parentObject.GetType().GetProperty(propertyName).PropertyType;
            if (propertyType == typeof(User))
            {
                propertyType = Configuration.Instance.UserType;
            }
            return Activator.CreateInstance(propertyType) as Entity;
        }
        private void AddParameter(SqlCommand command, string parameterName, object value, SqlDbType dbType, int size)
        {
            SqlParameter parameter = command.Parameters.Add(parameterName, dbType);
            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = value;
            }
            parameter.Size = size;
        }
        private void AddOutputParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size)
        {
            SqlParameter parameter = command.Parameters.Add(parameterName, dbType);
            parameter.Direction = ParameterDirection.Output;
            parameter.Size = size;
        }
        private SqlParameter CreateReturnParameter()
        {
            SqlParameter returnParameter = new SqlParameter();
            returnParameter.ParameterName = "@ReturnValue";
            returnParameter.SqlDbType = SqlDbType.Int;
            returnParameter.Size = 4;
            returnParameter.Direction = ParameterDirection.ReturnValue;
            return returnParameter;
        }
        private void GetUpdatePartAndParameterFormat(Request request, ICondition condition, SqlCommand sqlCommand, ref string updatePart, ref string parameterFormat)
        {
            string format = ", N'{0}', {1}";
            string format1 = "{0} {1}";
            string format2 = "{0}";
            string format3 = "{0} = {1}";
            List<string> formatItemList1 = new List<string>();
            List<string> formatItemList2 = new List<string>();
            List<string> formatItemList3 = new List<string>();
            TableField tableField = null;
            string parameterName = null;

            foreach (UpdatePropertyEntry entry in request.UpdatePropertyEntryList)
            {
                tableField = Configuration.Instance.GetTableField(request.Entity.GetType(), entry.EntityPropertyPath);
                parameterName = "@" + string.Join("", Guid.NewGuid().ToString().Split(new char[] { '-' }));
                formatItemList1.Add(string.Format(format1, parameterName, tableField.Type));
                formatItemList2.Add(string.Format(format2, parameterName));
                formatItemList3.Add(string.Format(format3, tableField.Name, parameterName));
                AddParameter(sqlCommand, parameterName, entry.PropertyValue, (SqlDbType)Enum.Parse(typeof(SqlDbType), tableField.TypeEnum), int.Parse(tableField.Size));
            }
            ProcessCondition(request, condition, sqlCommand, ref formatItemList1, ref formatItemList2);

            updatePart = string.Join(",", formatItemList3.ToArray());
            if (formatItemList2.Count > 0)
            {
                parameterFormat = string.Format(format, string.Join(",", formatItemList1.ToArray()), string.Join(",", formatItemList2.ToArray()));
            }
        }
        private string GetParametersFormat(Request request, ICondition condition, SqlCommand sqlCommand)
        {
            string parametersFormat = string.Empty;
            ProcessConditions(request, condition, sqlCommand, ref parametersFormat);
            return parametersFormat;
        }
        private void ProcessConditions(Request request, ICondition condition, SqlCommand sqlCommand, ref string parametersFormat)
        {
            if (request == null || condition == null)
            {
                return;
            }

            List<string> formatItemList1 = new List<string>();
            List<string> formatItemList2 = new List<string>();
            string format = ", N'{0}', {1}";

            ProcessCondition(request, condition, sqlCommand, ref formatItemList1, ref formatItemList2);
            if (formatItemList2.Count > 0)
            {
                parametersFormat = string.Format(format, string.Join(",", formatItemList1.ToArray()), string.Join(",", formatItemList2.ToArray()));
            }
        }
        private void ProcessCondition(Request request, ICondition condition, SqlCommand sqlCommand, ref List<string> formatItemList1, ref List<string> formatItemList2)
        {
            if (condition is IFieldCondition)
            {
                if (((IFieldCondition)condition).DbOperator == "In")
                {
                    return;
                }
                if (condition.IsValid(request))
                {
                    string format1 = "{0} {1}";
                    string format2 = "{0}";
                    IFieldCondition fieldCondition = (IFieldCondition)condition;
                    TableField tableField = GetTableField(request.Entity.GetType(), fieldCondition);

                    string dbType = tableField.Type;
                    SqlDbType sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), tableField.TypeEnum);
                    int fieldSize = int.Parse(tableField.Size);
                    object fieldValue = fieldCondition.GetConditionValue(request);
                    string parameterName = fieldCondition.ParameterName;

                    formatItemList1.Add(string.Format(format1, parameterName, dbType));
                    formatItemList2.Add(string.Format(format2, parameterName));

                    AddParameter(sqlCommand, parameterName, fieldValue, sqlDbType, fieldSize);
                }
            }
            else
            {
                foreach (ICondition childCondition in condition.ChildConditions)
                {
                    if (childCondition.IsValid(request))
                    {
                        ProcessCondition(request, childCondition, sqlCommand, ref formatItemList1, ref formatItemList2);
                    }
                }
            }
        }
        private TableField GetTableField(Type entityType, IFieldCondition fieldCondition)
        {
            string entityPropertyPath = fieldCondition.PropertyPath;
            if (entityPropertyPath.StartsWith("Data."))
            {
                entityPropertyPath = entityPropertyPath.Substring(5);
            }
            return Configuration.Instance.GetTableField(entityType, entityPropertyPath);
        }
        private ICondition GetCondition(Request request)
        {
            if (request.Condition != null)
            {
                return request.Condition;
            }

            ICondition condition = null;
            List<object> processedObjects = new List<object>();
            foreach (PropertyInfo propertyInfo in request.GetType().GetProperties())
            {
                ProcessComplexCondition(request, propertyInfo.Name, ref condition, propertyInfo.Name, processedObjects);
            }
            return condition;
        }
        private void ProcessComplexCondition(object parentObject, string propertyName, ref ICondition condition, string propertyPath, List<object> processedObjects)
        {
            object propertyValue = null;
            Entity entityValue = null;
            PropertyInfo propertyInformation = parentObject.GetType().GetProperty(propertyName);

            propertyValue = propertyInformation.GetValue(parentObject, null);

            if (propertyValue == null || propertyValue is EntityList || !(typeof(Property).IsAssignableFrom(propertyValue.GetType()) || typeof(Entity).IsAssignableFrom(propertyValue.GetType())) || processedObjects.Contains(propertyValue))
            {
                return;
            }

            processedObjects.Add(propertyValue);

            entityValue = propertyValue as Entity;

            if (propertyValue is Property)
            {
                ProcessSimpleProperty(propertyInformation, (Property)propertyValue, ref condition, propertyPath);
            }
            else if (entityValue != null)
            {
                foreach (PropertyInfo propertyInfo in entityValue.GetType().GetProperties())
                {
                    ProcessComplexCondition(entityValue, propertyInfo.Name, ref condition, string.IsNullOrEmpty(propertyPath) ? propertyInfo.Name : propertyPath + "." + propertyInfo.Name, processedObjects);
                }
            }
        }
        private void ProcessSimpleProperty(PropertyInfo propertyInfo, Property property, ref ICondition condition, string propertyPath)
        {
            if (property == null)
            {
                return;
            }

            IDataValidator dataValidator = property.GetDefaultValidator();
            IFieldCondition currentPropertyFieldCondition = null;
            object propertyValue = property.ObjectValue;
            object[] validatorAttributes = propertyInfo.GetCustomAttributes(typeof(ValidatorAttribute), true);

            if (validatorAttributes.Length > 0)
            {
                dataValidator = (IDataValidator)Activator.CreateInstance(((ValidatorAttribute)validatorAttributes[0]).ValidatorType);
            }

            if (property.Condition != null)
            {
                if (property.Condition.Value != null)
                {
                    propertyValue = property.Condition.Value;
                }
                if (property.Condition.DataValidatorType != null)
                {
                    dataValidator = (IDataValidator)Activator.CreateInstance(property.Condition.DataValidatorType);
                }
                string dbOperator = string.IsNullOrEmpty(property.Condition.DbOperator) ? "=" : property.Condition.DbOperator;
                currentPropertyFieldCondition = new FieldCondition(dataValidator.GetType(), propertyPath, propertyValue, dbOperator);
            }

            if (propertyValue == null)
            {
                return;
            }

            if (dataValidator != null && dataValidator.Validate(propertyValue))
            {
                currentPropertyFieldCondition = currentPropertyFieldCondition == null ? new FieldCondition(dataValidator.GetType(), propertyPath) : currentPropertyFieldCondition;

                if (condition == null)
                {
                    condition = currentPropertyFieldCondition;
                }
                else if (condition.GetType() == typeof(AndCondition))
                {
                    condition.ChildConditions.Add(currentPropertyFieldCondition);
                }
                else if (condition.GetType() == typeof(FieldCondition))
                {
                    ICondition andCondition = new AndCondition();
                    andCondition.ChildConditions.Add(condition);
                    andCondition.ChildConditions.Add(currentPropertyFieldCondition);
                    condition = andCondition;
                }
            }
        }

        #region GetEntityList Related Helper Functions

        private void ProcessReturnEntity(Request request, object parentObject, string propertyName, ReturnEntity returnEntity, IDataReader reader)
        {
            switch (returnEntity.EntityReturnMode)
            {
                case EntityReturnMode.Single:
                    if (reader.Read())
                    {
                        PopulateEntity(returnEntity.EntityType, reader, returnEntity);
                        UpdateParentObject(parentObject, propertyName, returnEntity.Entity);
                    }
                    break;
                case EntityReturnMode.Multiple:
                    EntityList entityList = new EntityList();
                    while (reader.Read())
                    {
                        PopulateEntity(returnEntity.EntityType, reader, returnEntity);
                        entityList.Add(returnEntity.Entity);
                    }
                    UpdateParentObject(parentObject, propertyName, entityList);
                    return;
            }
            foreach (ReturnEntity childReturnEntity in returnEntity.ChildReturnEntityList)
            {
                reader.NextResult();
                ProcessReturnEntity(request, returnEntity.Entity, childReturnEntity.PropertyName, childReturnEntity, reader);
            }
        }
        private void ProcessGetEntityOrEntityListCustomizeCommand(SqlConnection connection, SqlTransaction transaction, Command command, Request request, Reply reply)
        {
            try
            {
                SqlCommand sqlCommand = GetSqlCommand(connection, command, request);
                sqlCommand.CommandTimeout = commandTimeout;
                sqlCommand.Transaction = transaction;
                IDataReader reader = sqlCommand.ExecuteReader();

                foreach (ReturnEntity returnEntity in command.ReturnEntities)
                {
                    if (returnEntity.EntityType == request.Entity.GetType() && string.IsNullOrEmpty(returnEntity.PropertyName))
                    {
                        ProcessReturnEntity(request, reply, "Entity", returnEntity, reader);
                    }
                    else
                    {
                        ProcessReturnEntity(request, reply, returnEntity.PropertyName, returnEntity, reader);
                    }
                    reader.NextResult();
                }

                if (command.OutputParameters.Count > 0)
                {
                    SetOutputParameterValues(command.OutputParameters, sqlCommand, reply);
                }
                reader.Close();
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection(connection);
                }
            }
        }
        private void ProcessGetAllEntitiesSQL(SqlConnection connection, SqlTransaction transaction, Request request, Reply reply)
        {
            ReturnEntity returnEntity = null;
            try
            {
                EntityList entityList = new EntityList();
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandTimeout = commandTimeout;
                sqlCommand.Transaction = transaction;
                ICondition condition = GetCondition(request);
                returnEntity = request.ReturnEntity;
                string parametersFormat = GetParametersFormat(request, condition, sqlCommand);
                string orderSQL = string.Empty;
                string conditionExpression = string.Empty;

                if (condition != null && condition.IsValid(request))
                {
                    conditionExpression = condition.GetConditionExpression(request);
                }
                if (!string.IsNullOrEmpty(conditionExpression))
                {
                    conditionExpression = string.Format(" AND{0}", conditionExpression);
                }
                if (request.OrderFields.Count > 0)
                {
                    orderSQL = GetOrderSql(request);
                }

                sqlCommand.CommandText = string.Format(executeSQL, string.Format(selectAllEntitiesSQL, Configuration.Instance.GetTableName(request.Entity.GetType()), returnEntity.GetReturnFields(), conditionExpression, parametersFormat, orderSQL));

                IDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    PopulateEntity(returnEntity.EntityType, reader, returnEntity);
                    entityList.Add(returnEntity.Entity);
                }

                reader.Close();

                reply.TotalRecords = entityList.Count;

                if (string.IsNullOrEmpty(returnEntity.PropertyName))
                {
                    reply.Entity = entityList;
                }
                else
                {
                    UpdateParentObject(reply, returnEntity.PropertyName, entityList);
                }
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection(connection);
                }
            }
        }
        private void ProcessGetPagedEntityListSQL(SqlConnection connection, SqlTransaction transaction, Request request, Reply reply)
        {
            ReturnEntity returnEntity = null;
            try
            {
                EntityList entityList = new EntityList();
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandTimeout = commandTimeout;
                sqlCommand.Transaction = transaction;
                ICondition condition = GetCondition(request);
                returnEntity = request.ReturnEntity;
                string tableName = Configuration.Instance.GetTableName(request.Entity.GetType());
                string returnFields = returnEntity.GetReturnFields();
                int pageSize = request.PageSize;
                string orderSQL = GetDisOrderSql(request);
                string disOrderSQL = GetOrderSql(request);

                string parametersFormat1 = string.Empty;
                string parametersFormat2 = string.Empty;
                string parametersFormat3 = string.Empty;
                ProcessConditions(request, condition, sqlCommand, ref parametersFormat1, ref parametersFormat2, ref parametersFormat3);

                string conditionExpression = null;
                if (condition != null && condition.IsValid(request))
                {
                    conditionExpression = condition.GetConditionExpression(request);
                }
                string sqlFullPopulate = GetPopulateCountSql(request, conditionExpression);
                string sqlPopulate = GetPopulateSql(request, conditionExpression, returnFields);

                AddOutputParameter(sqlCommand, "@TotalRecords", SqlDbType.Int, 4);

                sqlCommand.CommandText = string.Format(selectEntityListSQL, sqlFullPopulate, sqlPopulate, parametersFormat1, parametersFormat2, parametersFormat3, pageSize, returnFields, orderSQL, disOrderSQL);

                IDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    PopulateEntity(returnEntity.EntityType, reader, returnEntity);
                    entityList.Add(returnEntity.Entity);
                }

                reader.Close();

                reply.TotalRecords = (int)sqlCommand.Parameters["@TotalRecords"].Value;
                EntityList fixedEntityList = Globals.FixLastPageBug(request.PageIndex.Value, request.PageSize, reply.TotalRecords, entityList);

                if (string.IsNullOrEmpty(returnEntity.PropertyName))
                {
                    reply.Entity = fixedEntityList;
                }
                else
                {
                    UpdateParentObject(reply, returnEntity.PropertyName, fixedEntityList);
                }
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection(connection);
                }
            }
        }
        private string GetOrderSql(Request request)
        {
            List<string> orderItemList = new List<string>();

            ReturnEntity returnEntity = request.ReturnEntity;

            PreProcessRequestOrderFields(request);
            foreach (OrderField field in request.OrderFields)
            {
                orderItemList.Add(string.Format("{0} {1}", string.Format("t.{0}", returnEntity.GetOrderFieldName(field.PropertyPath)), field.IsAscending ? "ASC" : "DESC"));
            }

            return string.Format(" ORDER BY {0} ", string.Join(",", orderItemList.ToArray()));
        }
        private string GetDisOrderSql(Request request)
        {
            List<string> disOrderItemList = new List<string>();

            ReturnEntity returnEntity = request.ReturnEntity;

            PreProcessRequestOrderFields(request);
            foreach (OrderField field in request.OrderFields)
            {
                disOrderItemList.Add(string.Format("{0} {1}", returnEntity.GetOrderFieldName(field.PropertyPath), !field.IsAscending ? "ASC" : "DESC"));
            }

            return string.Format("ORDER BY {0}", string.Join(",", disOrderItemList.ToArray()));
        }
        private string GetPopulateSql(Request request, string conditionSql, string returnFields)
        {
            string selectSql = string.Format("SELECT TOP {1} {2} FROM {0} t WHERE 1 = 1", Configuration.Instance.GetTableName(request.Entity.GetType()), request.PageIndex * request.PageSize, returnFields);
            string orderSql = GetOrderSql(request);

            if (!string.IsNullOrEmpty(conditionSql))
            {
                return selectSql + string.Format(" AND{0}", conditionSql) + orderSql;
            }
            return selectSql + orderSql;
        }
        private string GetPopulateCountSql(Request request, string conditionSql)
        {
            string selectCountSql = "SELECT t.EntityId FROM " + Configuration.Instance.GetTableName(request.Entity.GetType()) + " t WHERE 1 = 1";

            if (!string.IsNullOrEmpty(conditionSql))
            {
                return selectCountSql + string.Format(" AND{0}", conditionSql);
            }
            return selectCountSql;
        }
        private void ProcessConditions(Request request, ICondition condition, SqlCommand sqlCommand, ref string parametersFormat1, ref string parametersFormat2, ref string parametersFormat3)
        {
            if (request == null || condition == null)
            {
                return;
            }

            List<string> formatItemList1 = new List<string>();
            List<string> formatItemList2 = new List<string>();
            List<string> formatItemList3 = new List<string>();
            List<string> formatItemList4 = new List<string>();
            string format = ", N'{0}', {1}";

            ProcessCondition(request, condition, sqlCommand, ref formatItemList1, ref formatItemList2, ref formatItemList3, ref formatItemList4);

            parametersFormat1 = string.Join("", formatItemList1.ToArray());
            parametersFormat2 = string.Join("", formatItemList2.ToArray());
            if (formatItemList4.Count > 0)
            {
                parametersFormat3 = string.Format(format, string.Join(",", formatItemList3.ToArray()), string.Join(",", formatItemList4.ToArray()));
            }
        }
        private void ProcessCondition(Request request, ICondition condition, SqlCommand sqlCommand, ref List<string> formatItemList1, ref List<string> formatItemList2, ref List<string> formatItemList3, ref List<string> formatItemList4)
        {
            if (condition is IFieldCondition)
            {
                if (((IFieldCondition)condition).DbOperator == "In")
                {
                    return;
                }
                if (condition.IsValid(request))
                {
                    string format1 = "{0} {1}, ";
                    string format2 = "{0}, ";
                    string format3 = "{0} {1}";
                    string format4 = "{0}";

                    IFieldCondition fieldCondition = (IFieldCondition)condition;
                    TableField tableField = GetTableField(request.Entity.GetType(), fieldCondition);

                    string dbType = tableField.Type;
                    SqlDbType sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), tableField.TypeEnum);
                    int fieldSize = int.Parse(tableField.Size);
                    object fieldValue = fieldCondition.GetConditionValue(request);
                    string parameterName = fieldCondition.ParameterName;

                    formatItemList1.Add(string.Format(format1, parameterName, dbType));
                    formatItemList2.Add(string.Format(format2, parameterName));
                    formatItemList3.Add(string.Format(format3, parameterName, dbType));
                    formatItemList4.Add(string.Format(format4, parameterName));

                    AddParameter(sqlCommand, parameterName, fieldValue, sqlDbType, fieldSize);
                }
            }
            else
            {
                foreach (ICondition childCondition in condition.ChildConditions)
                {
                    if (childCondition.IsValid(request))
                    {
                        ProcessCondition(request, childCondition, sqlCommand, ref formatItemList1, ref formatItemList2, ref formatItemList3, ref formatItemList4);
                    }
                }
            }
        }
        private void PreProcessRequestOrderFields(Request request)
        {
            bool isEntityIdExist = false;
            foreach (OrderField orderField in request.OrderFields)
            {
                if (orderField.PropertyPath == "EntityId")
                {
                    isEntityIdExist = true;
                    break;
                }
            }
            if (!isEntityIdExist)
            {
                request.OrderFields.Add(new OrderField("EntityId", false));
            }
        } 

        #endregion

        #endregion
    }
}