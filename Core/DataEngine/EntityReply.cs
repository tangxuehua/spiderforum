namespace System.Web.Core
{
    public class Reply
    {
        #region Private Members

        private Entity entity = null;
        private int totalRecords = 0;

        #endregion

        #region Public Properties

        public Entity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
            }
        }
        public EntityList EntityList
        {
            get
            {
                return Entity as EntityList;
            }
        }
        public int TotalRecords
        {
            get
            {
                return totalRecords;
            }
            set
            {
                totalRecords = value;
            }
        }

        #endregion
    }
}