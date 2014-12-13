using System;

namespace StreamSource.Storage
{
    public class StreamRecord
    {
        public readonly byte[] StreamId;
        public readonly int StreamExpectedVersion;
        public readonly string StreamName;
        public readonly Guid MessageId;
        public readonly byte[] MessageData;
        public readonly string MessageDataContract;
        public readonly byte[] MessageMetadata;
        public readonly int MessageOrdinal;
        public readonly Guid CausationId;
        public readonly Guid CorrelationId;

        public StreamRecord(byte[] streamId, string streamName, int streamExpectedVersion, Guid messageId, byte[] messageData, string messageDataContract, byte[] messageMetadata, int messageOrdinal, Guid causationId, Guid correlationId)
        {
            if (streamId == null) throw new ArgumentNullException("streamId");
            if (streamExpectedVersion < 0) throw new ArgumentOutOfRangeException("streamExpectedVersion", streamExpectedVersion, "The expected version must be greater than or equal to 0.");
            if (streamName == null) throw new ArgumentNullException("streamName");
            if (messageData == null) throw new ArgumentNullException("messageData");
            if (messageDataContract == null) throw new ArgumentNullException("messageDataContract");
            if (messageMetadata == null) throw new ArgumentNullException("messageMetadata");
            if (messageOrdinal < 0) throw new ArgumentOutOfRangeException("messageOrdinal", messageOrdinal, "The message ordinal must be greater than or equal to 0.");
            StreamId = streamId;
            StreamExpectedVersion = streamExpectedVersion;
            StreamName = streamName;
            MessageId = messageId;
            MessageData = messageData;
            MessageDataContract = messageDataContract;
            MessageMetadata = messageMetadata;
            MessageOrdinal = messageOrdinal;
            CausationId = causationId;
            CorrelationId = correlationId;
        }
    }
}
