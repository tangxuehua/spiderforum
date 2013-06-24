using System.Collections.Generic;

namespace System.Web.Core
{
    public class OrChildCondition : IChildCondition
    {
        #region Private Members

        private List<IChildCondition> childConditions = new List<IChildCondition>();

        #endregion

        #region IChildCondition ≥…‘±

        public List<IChildCondition> ChildConditions
        {
            get
            {
                return childConditions;
            }
        }
        public bool IsValid(Request request)
        {
            if (ChildConditions.Count < 2)
            {
                return false;
            }
            foreach (IChildCondition childCondition in ChildConditions)
            {
                if (childCondition.IsValid(request))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
