using System.Collections.Generic;

namespace System.Web.Core
{
    public class Entity
    {
        #region Private Members

        private IntType entityId = new IntType();
        private ExtendedAttributes extendedAttributes = new ExtendedAttributes();

        #endregion

        #region Constructors

        public Entity()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public IntType EntityId
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
        public ExtendedAttributes ExtendedAttributes
        {
            get
            {
                return this.extendedAttributes;
            }
        }

        #endregion

        #region Public Methods

        public virtual void Initialize()
        {
        }
        public virtual bool IsOwner(User user)
        {
            return false;
        }

        #endregion

        #region Protected Methods

        protected TValue GetParameterValue<TValue>(object name)
        {
            return UrlManager.Instance.GetParameterValue<TValue>(name);
        }

        #endregion

        #region Private Methods

        private bool Validate(Entity entity, long permission, User user)
        {
            if (entity == null || user == null)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}