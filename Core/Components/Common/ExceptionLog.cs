using System;

namespace System.Web.Core
{
    public class ExceptionLog : Entity
    {
        #region Private Members

        private StringType message = new StringType();
        private StringType exceptionDetails = new StringType();
        private StringType ipAddress = new StringType();
        private StringType userAgent = new StringType();
        private StringType httpReferrer = new StringType();
        private StringType httpVerb = new StringType();
        private StringType pathAndQuery = new StringType();
        private DateTimeType dateCreated = new DateTimeType();
        private DateTimeType dateLastOccurred = new DateTimeType();
        private IntType frequency = new IntType();
        private StringType userName = new StringType();

        #endregion

        #region Public Properties

        public StringType Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
        public StringType ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
            set
            {
                exceptionDetails = value;
            }
        }
        public StringType IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }
        public StringType UserAgent
        {
            get
            {
                return userAgent;
            }
            set
            {
                userAgent = value;
            }
        }
        public StringType HttpReferrer
        {
            get
            {
                return httpReferrer;
            }
            set
            {
                httpReferrer = value;
            }
        }
        public StringType HttpVerb
        {
            get
            {
                return httpVerb;
            }
            set
            {
                httpVerb = value;
            }
        }
        public StringType PathAndQuery
        {
            get
            {
                return pathAndQuery;
            }
            set
            {
                pathAndQuery = value;
            }
        }
        public DateTimeType DateCreated
        {
            get
            {
                return dateCreated;
            }
            set
            {
                dateCreated = value;
            }
        }
        public DateTimeType DateLastOccurred
        {
            get
            {
                return dateLastOccurred;
            }
            set
            {
                dateLastOccurred = value;
            }
        }
        public IntType Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }
        public StringType UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        #endregion
    }
}