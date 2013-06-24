using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace System.Web.Core
{
    public class Request
    {
        #region Private Members

        private List<UpdatePropertyEntry> updatePropertyEntryList = new List<UpdatePropertyEntry>();
        private OrderFields orderFields = new OrderFields();
        private List<RequestDependency> requestDependencies = new List<RequestDependency>();
        private int entityId = 0;
        private Entity entity = null;
        private string commandIdent = null;
        private OperationType operationType;
        private int? pageIndex = null;
        private int pageSize = 20;
        private bool getAll = false;
        private ICondition condition = null;
        private ReturnEntity returnEntity = null;

        #endregion

        #region Constructors

        public Request()
        {
            PageIndex = UrlManager.Instance.GetParameterValue<int?>(ParameterName.PageIndex);
            if (!PageIndex.HasValue || PageIndex.Value < 1)
            {
                PageIndex = 1;
            }
        }

        #endregion

        #region Public Properties

        public int EntityId
        {
            get
            {
                return entityId;
            }
            set
            {
                entityId = value;
            }
        }
        public Entity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
                ReturnEntity = GetReturnEntity();
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
        public string CommandIdent
        {
            get
            {
                return commandIdent;
            }
            set
            {
                commandIdent = value;
            }
        }
        public OperationType OperationType
        {
            get
            {
                return operationType;
            }
            set
            {
                operationType = value;
            }
        }
        public int? PageIndex
        {
            get
            {
                if (!pageIndex.HasValue || pageIndex < 1)
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }
        public bool GetAll
        {
            get
            {
                return getAll;
            }
            set
            {
                getAll = value;
            }
        }
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
        public List<UpdatePropertyEntry> UpdatePropertyEntryList
        {
            get
            {
                return updatePropertyEntryList;
            }
        }
        public OrderFields OrderFields
        {
            get
            {
                return orderFields;
            }
        }
        public List<RequestDependency> RequestDependencies
        {
            get
            {
                return requestDependencies;
            }
        }

        #endregion

        #region Public Methods

        public void RemoveReturnPropertyPath(params string[] propertyPaths)
        {
            foreach (string propertyPath in propertyPaths)
            {
                if (!string.IsNullOrEmpty(propertyPath))
                {
                    returnEntity.RemoveReturnPropertyPath(propertyPath);
                }
            }
        }

        #endregion

        #region Protected Methods

        protected TValue GetParameterValue<TValue>(object name)
        {
            return UrlManager.Instance.GetParameterValue<TValue>(name);
        }

        #endregion

        #region Private Methods

        private ReturnEntity GetReturnEntity()
        {
            return Configuration.Instance.GetEntityMapping(this.Entity.GetType()).GetDefaultReturnEntity();
        }

        #endregion
    }
    public class TRequest<TEntity> : Request where TEntity : Entity, new()
    {
        public TRequest()
        {
            Data = new TEntity();
        }
        public TRequest(int entityId) : this()
        {
            EntityId = entityId;
        }

        public TEntity Data
        {
            get
            {
                return base.Entity as TEntity;
            }
            set
            {
                base.Entity = value;
            }
        }
    }
}