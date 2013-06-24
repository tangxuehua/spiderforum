using System;
using System.Web.Core;

namespace Forum.Business
{
    public class Thread : Entity
    {
        #region Private Members

        private StringType subject = new StringType();
        private StringType body = new StringType();
        private IntType authorId = new IntType();
        private StringType author = new StringType();
        private IntType mostRecentReplierId = new IntType();
        private StringType mostRecentReplierName = new StringType();
        private NDateTimeType createDate = new NDateTimeType();
        private NDateTimeType updateDate = new NDateTimeType();
        private NDateTimeType stickDate = new NDateTimeType();
        private IntType groupId = new IntType();
        private IntType sectionId = new IntType();
        private IntType threadStatus = new IntType();
        private IntType totalPosts = new IntType();
        private IntType totalViews = new IntType();
        private IntType threadMarks = new IntType();
        private IntType threadReleaseStatus = new IntType();

        #endregion

        #region Public Properties

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
        public StringType Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
            }
        }
        public NDateTimeType CreateDate
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
        public NDateTimeType UpdateDate
        {
            get
            {
                return updateDate;
            }
            set
            {
                updateDate = value;
            }
        }
        public NDateTimeType StickDate
        {
            get
            {
                return stickDate;
            }
            set
            {
                stickDate = value;
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
        public IntType TotalPosts
        {
            get
            {
                return totalPosts;
            }
            set
            {
                totalPosts = value;
            }
        }
        public IntType TotalViews
        {
            get
            {
                return totalViews;
            }
            set
            {
                totalViews = value;
            }
        }
        [Validator(typeof(ThreadStatusValidator))]
        public IntType ThreadStatus
        {
            get
            {
                return threadStatus;
            }
            set
            {
                threadStatus = value;
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
        public IntType MostRecentReplierId
        {
            get
            {
                return mostRecentReplierId;
            }
            set
            {
                mostRecentReplierId = value;
            }
        }
        public IntType ThreadMarks
        {
            get
            {
                return threadMarks;
            }
            set
            {
                threadMarks = value;
            }
        }
        [Validator(typeof(ThreadReleaseStatusValidator))]
        public IntType ThreadReleaseStatus
        {
            get
            {
                return threadReleaseStatus;
            }
            set
            {
                threadReleaseStatus = value;
            }
        }
        public StringType MostRecentReplierName
        {
            get
            {
                return mostRecentReplierName;
            }
            set
            {
                mostRecentReplierName = value;
            }
        }

        #endregion

        #region Public Methods

        public override bool IsOwner(User user)
        {
            if (user.EntityId.Value == AuthorId.Value)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}