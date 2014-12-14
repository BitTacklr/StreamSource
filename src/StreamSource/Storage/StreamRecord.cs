using System;
using System.Linq;

namespace StreamSource.Storage
{
    public class StreamRecord
    {
        public readonly byte[] StreamId;
        public readonly int StreamVersion;
        public readonly string StreamName;
        public readonly Guid MessageId;
        public readonly byte[] MessageData;
        public readonly string MessageDataContract;
        public readonly byte[] MessageMetadata;
        public readonly int MessageOrdinal;
        public readonly Guid CausationId;
        public readonly Guid CorrelationId;

        public StreamRecord(byte[] streamId, string streamName, int streamVersion, Guid messageId, byte[] messageData, string messageDataContract, byte[] messageMetadata, int messageOrdinal, Guid causationId, Guid correlationId)
        {
            if (streamId == null) throw new ArgumentNullException("streamId");
            if (streamVersion < 0) throw new ArgumentOutOfRangeException("streamVersion", streamVersion, "The stream version must be greater than or equal to 0.");
            if (streamName == null) throw new ArgumentNullException("streamName");
            if (messageData == null) throw new ArgumentNullException("messageData");
            if (messageDataContract == null) throw new ArgumentNullException("messageDataContract");
            if (messageMetadata == null) throw new ArgumentNullException("messageMetadata");
            if (messageOrdinal < 0) throw new ArgumentOutOfRangeException("messageOrdinal", messageOrdinal, "The message ordinal must be greater than or equal to 0.");
            StreamId = streamId;
            StreamVersion = streamVersion;
            StreamName = streamName;
            MessageId = messageId;
            MessageData = messageData;
            MessageDataContract = messageDataContract;
            MessageMetadata = messageMetadata;
            MessageOrdinal = messageOrdinal;
            CausationId = causationId;
            CorrelationId = correlationId;
        }

        public bool Equals(StreamRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            return StreamId.SequenceEqual(other.StreamId) &&
                StreamVersion == other.StreamVersion &&
                string.Equals(StreamName, other.StreamName) &&
                MessageId == other.MessageId &&
                MessageData.SequenceEqual(other.MessageData) &&
                string.Equals(MessageDataContract, other.MessageDataContract) &&
                MessageMetadata.SequenceEqual(other.MessageMetadata) &&
                MessageOrdinal == other.MessageOrdinal &&
                CausationId == other.CausationId &&
                CorrelationId == other.CorrelationId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StreamRecord)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = StreamId.Aggregate(0, (current, next) => current ^ next);
            hashCode ^= StreamVersion;
            hashCode ^= StreamName.GetHashCode();
            hashCode ^= MessageId.GetHashCode();
            hashCode ^= MessageData.Aggregate(0, (current, next) => current ^ next);
            hashCode ^= MessageDataContract.GetHashCode();
            hashCode ^= MessageMetadata.Aggregate(0, (current, next) => current ^ next);
            hashCode ^= MessageOrdinal;
            hashCode ^= CausationId.GetHashCode();
            hashCode ^= CorrelationId.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StreamRecord left, StreamRecord right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StreamRecord left, StreamRecord right)
        {
            return !Equals(left, right);
        }
    }
}
