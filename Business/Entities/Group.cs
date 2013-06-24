using System.Web.Core;

namespace Forum.Business
{
    public class Group : Entity
    {
        private StringType subject = new StringType();
        private IntType enabled = new IntType();

        public StringType Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }
        [Validator(typeof(EnableValidator))]
        public IntType Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }
    }
}