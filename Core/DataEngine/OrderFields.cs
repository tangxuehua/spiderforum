using System.Collections.Generic;

namespace System.Web.Core
{
    public class OrderFields : List<OrderField>
    {
        public void AddField(OrderField item)
        {
            foreach (OrderField field in this)
            {
                if (field.PropertyPath == item.PropertyPath)
                {
                    return;
                }
            }
            Add(item);
        }
        public void AddRangeFields(IEnumerable<OrderField> collection)
        {
            if (collection == null)
            {
                return;
            }
            foreach (OrderField item in collection)
            {
                AddField(item);
            }
        }
    }
}