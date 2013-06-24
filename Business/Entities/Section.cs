using System;
using System.Collections.Generic;
using System.Web.Core;

namespace Forum.Business
{
    public class Section : Entity
    {
        private StringType subject = new StringType();
        private IntType totalThreads = new IntType();
        private IntType groupId = new IntType();
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
        public IntType GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                groupId = value;
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
        public IntType TotalThreads
        {
            get
            {
                return totalThreads;
            }
            set
            {
                totalThreads = value;
            }
        }
    }
}