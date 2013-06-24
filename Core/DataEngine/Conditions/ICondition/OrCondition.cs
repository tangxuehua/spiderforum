using System.Collections.Generic;

namespace System.Web.Core
{
    public class OrCondition : ICondition
    {
        #region Private Members

        private List<ICondition> childConditions = new List<ICondition>();

        #endregion

        #region IFieldCondition ≥…‘±

        public List<ICondition> ChildConditions
        {
            get
            {
                return childConditions;
            }
        }
        public bool IsValid(Request request)
        {
            foreach (ICondition childCondition in ChildConditions)
            {
                if (childCondition.IsValid(request))
                {
                    return true;
                }
            }
            return false;
        }
        public string GetConditionExpression(Request request)
        {
            string seperator = " OR";
            List<string> items = new List<string>();
            foreach (ICondition childCondition in ChildConditions)
            {
                if (childCondition.IsValid(request))
                {
                    items.Add(childCondition.GetConditionExpression(request));
                }
            }
            if (items.Count > 0)
            {
                return " (" + string.Join(seperator, items.ToArray()).Substring(1) + ")";
            }
            return null;
        }
        public string GetConditionExpressionWithoutAliasName(Request request)
        {
            string seperator = " OR";
            List<string> items = new List<string>();
            foreach (ICondition childCondition in ChildConditions)
            {
                if (childCondition.IsValid(request))
                {
                    items.Add(childCondition.GetConditionExpressionWithoutAliasName(request));
                }
            }
            if (items.Count > 0)
            {
                return " (" + string.Join(seperator, items.ToArray()).Substring(1) + ")";
            }
            return null;
        }
        public bool HasInOperatorCondition
        {
            get
            {
                foreach (ICondition childCondition in ChildConditions)
                {
                    if (childCondition.HasInOperatorCondition)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion
    }
}
