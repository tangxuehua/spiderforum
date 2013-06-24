namespace System.Web.Core
{
    public abstract class RequestBinder
    {
        #region Private Members

        private Request request;
        private Reply reply;
        private IReplyConverter replyConverter;

        #endregion

        #region Constructors

        public RequestBinder(Request request)
        {
            this.request = request;
        }
        public RequestBinder(IReplyConverter replyConverter, Request request) : this(request)
        {
            this.replyConverter = replyConverter;
        }

        #endregion

        #region Public Properties

        public Request Request
        {
            get
            {
                return request;
            }
            set
            {
                request = value;
            }
        }
        public Reply Reply
        {
            get
            {
                return reply;
            }
            internal set
            {
                reply = value;
            }
        }
        public IReplyConverter ReplyConverter
        {
            get
            {
                return replyConverter;
            }
            set
            {
                replyConverter = value;
            }
        }

        #endregion
    }
    public class TRequestBinder<TReply> : RequestBinder where TReply : Reply, new()
    {
        #region Constructors

        public TRequestBinder(Request request) : this(null, request)
        {
        }
        public TRequestBinder(IReplyConverter replyConverter, Request request) : base(replyConverter, request)
        {
            Reply = new TReply(); 
        }

        #endregion
    }
}