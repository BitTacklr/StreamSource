using System;

namespace StreamSource
{
    public class StreamChange
    {
        public readonly Guid MessageId;
        public readonly object Message;
        public readonly Metadatum[] Metadata;

        public StreamChange(Guid messageId, object message, Metadatum[] metadata)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (metadata == null) throw new ArgumentNullException("metadata");
            MessageId = messageId;
            Message = message;
            Metadata = metadata;
        }
    }
}