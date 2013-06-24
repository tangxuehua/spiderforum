using System;
using System.Web.Core;

namespace Forum.Business
{
    public class Post : Entity
    {
        private IntType groupId = new IntType();
        private IntType sectionId = new IntType();
        private IntType threadId = new IntType();
        private StringType body = new StringType();
        private IntType authorId = new IntType();
        private DateTimeType createDate = new DateTimeType();
        private int postIndex = 0;

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
        public IntType SectionId
        {
            get
            {
                return sectionId;
            }
            set
            {
                sectionId = value;
            }
        }
        public IntType ThreadId
        {
            get
            {
                return threadId;
            }
            set
            {
                threadId = value;
            }
        }
        public IntType AuthorId
        {
            get
            {
                return authorId;
            }
            set
            {
                authorId = value;
            }
        }
        public StringType Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }
        public DateTimeType CreateDate
        {
            get
            {
                return createDate;
            }
            set
            {
                createDate = value;
            }
        }
        public int PostIndex
        {
            get
            {
                return postIndex;
            }
            set
            {
                postIndex = value;
            }
        }

        public override bool IsOwner(User user)
        {
            if (user.EntityId.Value == AuthorId.Value)
            {
                return true;
            }
            return false;
        }
    }
}