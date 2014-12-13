using System;
using System.Linq;

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

        public bool Equals(StreamChange other)
        {
            if (ReferenceEquals(other, null)) return false;
            return MessageId == other.MessageId &&
                   Equals(Message, other.Message) &&
                   Metadata.SequenceEqual(other.Metadata);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return other is StreamChange && Equals((StreamChange)other);
        }

        public override int GetHashCode()
        {
            return Metadata.Aggregate(
                MessageId.GetHashCode() ^ Message.GetHashCode(),
                (current, next) => current ^ next.GetHashCode());
        }

        public static bool operator ==(StreamChange left, StreamChange right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StreamChange left, StreamChange right)
        {
            return !Equals(left, right);
        }
    }
}