using System.Collections.Generic;

namespace System.Web.Core
{
    public delegate void NeedReplyEventHandler(IReplyConverter replyConverter);

    public interface IReplyConverter
    {
        event NeedReplyEventHandler NeedReply;
        void GetReplies(List<RequestBinder> requestBinders);
    }
}