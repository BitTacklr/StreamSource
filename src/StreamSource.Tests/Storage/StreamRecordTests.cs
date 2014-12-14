using System;
using NUnit.Framework;
using StreamSource.Framework;

namespace StreamSource.Storage
{
    [TestFixture]
    public class StreamRecordTests
    {
        [Test]
        public void StreamIdCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => StreamRecordBuilder.Default.WithStreamId(null).Build());
        }

        [Test]
        public void StreamNameCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => StreamRecordBuilder.Default.WithStreamName(null).Build());
        }

        [TestCase(Int32.MinValue)]
        [TestCase(-1)]
        public void StreamVersionCanNotBeLessThanZero(int version)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => StreamRecordBuilder.Default.WithStreamVersion(version).Build());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(Int32.MaxValue)]
        public void StreamVersionCanBeGreaterThanOrEqualToZero(int version)
        {
            Assert.DoesNotThrow(
                () => StreamRecordBuilder.Default.WithStreamVersion(version).Build());
        }

        [Test]
        public void MessageDataCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => StreamRecordBuilder.Default.WithMessageData(null).Build());
        }

        [Test]
        public void MessageDataContractCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => StreamRecordBuilder.Default.WithMessageDataContract(null).Build());
        }

        [Test]
        public void MessageMetadataCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => StreamRecordBuilder.Default.WithMessageMetadata(null).Build());
        }

        [TestCase(Int32.MinValue)]
        [TestCase(-1)]
        public void MessageOrdinalCanNotBeLessThanZero(int ordinal)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => StreamRecordBuilder.Default.WithMessageOrdinal(ordinal).Build());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(Int32.MaxValue)]
        public void MessageOrdinalCanBeGreaterThanOrEqualToZero(int ordinal)
        {
            Assert.DoesNotThrow(
                () => StreamRecordBuilder.Default.WithMessageOrdinal(ordinal).Build());
        }

        [Test]
        public void StreamIdReturnsExpectedResult()
        {
            var streamId = new byte[] {1, 2, 3};
            var sut = StreamRecordBuilder.Default.WithStreamId(streamId).Build();
            var result = sut.StreamId;
            Assert.That(result, Is.EquivalentTo(streamId));
        }

        [Test]
        public void StreamNameReturnsExpectedResult()
        {
            var streamName = Guid.NewGuid().ToString();
            var sut = StreamRecordBuilder.Default.WithStreamName(streamName).Build();
            var result = sut.StreamName;
            Assert.That(result, Is.EqualTo(streamName));
        }
        
        [Test]
        public void StreamVersionReturnsExpectedResult()
        {
            const int streamVersion = 123;
            var sut = StreamRecordBuilder.Default.WithStreamVersion(streamVersion).Build();
            var result = sut.StreamVersion;
            Assert.That(result, Is.EqualTo(streamVersion));
        }

        [Test]
        public void MessageIdReturnsExpectedResult()
        {
            var messageId = Guid.NewGuid();
            var sut = StreamRecordBuilder.Default.WithMessageId(messageId).Build();
            var result = sut.MessageId;
            Assert.That(result, Is.EqualTo(messageId));
        }

        [Test]
        public void MessageDataReturnsExpectedResult()
        {
            var messageData = new byte[] { 1, 2, 3 };
            var sut = StreamRecordBuilder.Default.WithMessageData(messageData).Build();
            var result = sut.MessageData;
            Assert.That(result, Is.EquivalentTo(messageData));
        }

        [Test]
        public void MessageDataContractReturnsExpectedResult()
        {
            var messageDataContract = Guid.NewGuid().ToString();
            var sut = StreamRecordBuilder.Default.WithMessageDataContract(messageDataContract).Build();
            var result = sut.MessageDataContract;
            Assert.That(result, Is.EqualTo(messageDataContract));
        }

        [Test]
        public void MessageMetadataReturnsExpectedResult()
        {
            var messageMetadata = new byte[] { 1, 2, 3 };
            var sut = StreamRecordBuilder.Default.WithMessageMetadata(messageMetadata).Build();
            var result = sut.MessageMetadata;
            Assert.That(result, Is.EquivalentTo(messageMetadata));
        }

        [Test]
        public void MessageOrdinalReturnsExpectedResult()
        {
            var messageOrdinal = new Random().Next();
            var sut = StreamRecordBuilder.Default.WithMessageOrdinal(messageOrdinal).Build();
            var result = sut.MessageOrdinal;
            Assert.That(result, Is.EqualTo(messageOrdinal));
        }

        [Test]
        public void CausationIdReturnsExpectedResult()
        {
            var causationId = Guid.NewGuid();
            var sut = StreamRecordBuilder.Default.WithCausationId(causationId).Build();
            var result = sut.CausationId;
            Assert.That(result, Is.EqualTo(causationId));
        }

        [Test]
        public void CorrelationIdReturnsExpectedResult()
        {
            var correlationId = Guid.NewGuid();
            var sut = StreamRecordBuilder.Default.WithCorrelationId(correlationId).Build();
            var result = sut.CorrelationId;
            Assert.That(result, Is.EqualTo(correlationId));
        }

        [Test]
        public void VerifyEquality()
        {
            var causationId1 = Guid.NewGuid();
            var causationId2 = Guid.NewGuid();
            var correlationId1 = Guid.NewGuid();
            var correlationId2 = Guid.NewGuid();
            var messageData1 = new byte[] { 1, 2, 3};
            var messageData2 = new byte[] {4, 5, 6};
            var messageDataContract1 = Guid.NewGuid().ToString();
            var messageDataContract2 = Guid.NewGuid().ToString();
            var messageId1 = Guid.NewGuid();
            var messageId2 = Guid.NewGuid();
            var messageMetadata1 = new byte[] { 1, 2, 3 };
            var messageMetadata2 = new byte[] { 4, 5, 6 };
            const int messageOrdinal1 = 1;
            const int messageOrdinal2 = 2;
            var streamId1 = new byte[] { 1, 2, 3 };
            var streamId2 = new byte[] { 4, 5, 6 };
            var streamName1 = Guid.NewGuid().ToString();
            var streamName2 = Guid.NewGuid().ToString();
            const int streamVersion1 = 1;
            const int streamVersion2 = 2;
            new EqualityAssertion(StreamRecordBuilder.Default.Build()).
                VerifyEqual(StreamRecordBuilder.Default.Build(), StreamRecordBuilder.Default.Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.Build(), new object()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithCausationId(causationId1).Build(), StreamRecordBuilder.Default.WithCausationId(causationId2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithCorrelationId(correlationId1).Build(), StreamRecordBuilder.Default.WithCorrelationId(correlationId2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithMessageData(messageData1).Build(), StreamRecordBuilder.Default.WithMessageData(messageData2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithMessageDataContract(messageDataContract1).Build(), StreamRecordBuilder.Default.WithMessageDataContract(messageDataContract2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithMessageId(messageId1).Build(), StreamRecordBuilder.Default.WithMessageId(messageId2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithMessageMetadata(messageMetadata1).Build(), StreamRecordBuilder.Default.WithMessageMetadata(messageMetadata2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithMessageOrdinal(messageOrdinal1).Build(), StreamRecordBuilder.Default.WithMessageOrdinal(messageOrdinal2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithStreamId(streamId1).Build(), StreamRecordBuilder.Default.WithStreamId(streamId2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithStreamName(streamName1).Build(), StreamRecordBuilder.Default.WithStreamName(streamName2).Build()).
                VerifyNotEqual(StreamRecordBuilder.Default.WithStreamVersion(streamVersion1).Build(), StreamRecordBuilder.Default.WithStreamVersion(streamVersion2).Build()).
                Assert();
        }

        class StreamRecordBuilder
        {
            public static readonly StreamRecordBuilder Default = 
                new StreamRecordBuilder(
                    new byte[0],
                    "",
                    0,
                    Guid.Empty,
                    new byte[0],
                    "",
                    new byte[0],
                    0,
                    Guid.Empty,
                    Guid.Empty);

            private readonly byte[] _streamId;
            private readonly string _streamName;
            private readonly int _streamVersion;
            private readonly Guid _messageId;
            private readonly byte[] _messageData;
            private readonly string _messageDataContract;
            private readonly byte[] _messageMetadata;
            private readonly int _messageOrdinal;
            private readonly Guid _causationId;
            private readonly Guid _correlationId;

            StreamRecordBuilder(byte[] streamId, string streamName, int streamVersion, Guid messageId, byte[] messageData, string messageDataContract, byte[] messageMetadata, int messageOrdinal, Guid causationId, Guid correlationId)
            {
                _streamId = streamId;
                _streamVersion = streamVersion;
                _streamName = streamName;
                _messageId = messageId;
                _messageData = messageData;
                _messageDataContract = messageDataContract;
                _messageMetadata = messageMetadata;
                _messageOrdinal = messageOrdinal;
                _causationId = causationId;
                _correlationId = correlationId;
            }

            public StreamRecordBuilder WithStreamId(byte[] value)
            {
                return new StreamRecordBuilder(
                    value, _streamName, _streamVersion,
                    _messageId, _messageData, _messageDataContract, _messageMetadata, 
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithStreamName(string value)
            {
                return new StreamRecordBuilder(
                    _streamId, value, _streamVersion,
                    _messageId, _messageData, _messageDataContract, _messageMetadata,
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithStreamVersion(int value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, value,
                    _messageId, _messageData, _messageDataContract, _messageMetadata,
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithMessageId(Guid value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    value, _messageData, _messageDataContract, _messageMetadata,
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithMessageData(byte[] value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    _messageId, value, _messageDataContract, _messageMetadata,
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithMessageDataContract(string value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    _messageId, _messageData, value, _messageMetadata,
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithMessageMetadata(byte[] value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    _messageId, _messageData, _messageDataContract, value,
                    _messageOrdinal, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithMessageOrdinal(int value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    _messageId, _messageData, _messageDataContract, _messageMetadata,
                    value, _causationId, _correlationId);
            }

            public StreamRecordBuilder WithCausationId(Guid value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    _messageId, _messageData, _messageDataContract, _messageMetadata,
                    _messageOrdinal, value, _correlationId);
            }

            public StreamRecordBuilder WithCorrelationId(Guid value)
            {
                return new StreamRecordBuilder(
                    _streamId, _streamName, _streamVersion,
                    _messageId, _messageData, _messageDataContract, _messageMetadata,
                    _messageOrdinal, _causationId, value);
            }

            public StreamRecord Build()
            {
                return new StreamRecord(
                    _streamId, _streamName, _streamVersion,
                    _messageId, _messageData, _messageDataContract, _messageMetadata,
                    _messageOrdinal, _causationId, _correlationId);
            }
        }
    }
}
