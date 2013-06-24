namespace System.Web.Core
{
    public abstract class AttachmentEntity : Entity
    {
        private StringType attachmentFileName = new StringType();
        private ByteType attachmentContent = new ByteType();

        public StringType AttachmentFileName
        {
            get
            {
                return attachmentFileName;
            }
            set
            {
                attachmentFileName = value;
            }
        }
        public ByteType AttachmentContent
        {
            get
            {
                return attachmentContent;
            }
            set
            {
                attachmentContent = value;
            }
        }
    }
}
