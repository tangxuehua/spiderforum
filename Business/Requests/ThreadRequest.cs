using System.Collections.Generic;
using System.Web.Core;

namespace Forum.Business
{
    public class ThreadRequest : TRequest<Thread>
    {
        private IntType orderField = new IntType();
        private IntType replierId = new IntType();

        public ThreadRequest()
        {
        }
        public ThreadRequest(int entityId) : base(entityId)
        {
        }

        public IntType OrderField
        {
            get
            {
                return orderField;
            }
            set
            {
                orderField = value;
            }
        }
        public IntType ReplierId
        {
            get
            {
                return replierId;
            }
            set
            {
                replierId = value;
            }
        }
    }
}