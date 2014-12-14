using System;
using NUnit.Framework;
using StreamSource.Framework;

namespace StreamSource
{
    [TestFixture]
    public class StreamChangeTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                StreamChangeBuilder.Default.WithMessage(null).Build());
        }

        [Test]
        public void MetadataCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                StreamChangeBuilder.Default.WithMetadata(null).Build());
        }

        [Test]
        public void MetadataCanBeEmpty()
        {
            Assert.DoesNotThrow(() =>
                StreamChangeBuilder.Default.WithMetadata(new Metadatum[0]));
        }

        [Test]
        public void MessageIdReturnsExpectedResult()
        {
            var messageId = Guid.NewGuid();
            var sut = StreamChangeBuilder.Default.WithMessageId(messageId).Build();
            var result = sut.MessageId;
            Assert.That(result, Is.EqualTo(messageId));
        }

        [Test]
        public void MessageReturnsExpectedResult()
        {
            var message = new object();
            var sut = StreamChangeBuilder.Default.WithMessage(message).Build();
            var result = sut.Message;
            Assert.That(result, Is.EqualTo(message));
        }

        [Test]
        public void MetadataReturnsExpectedResult()
        {
            var metadata = new[]
            {
                new Metadatum("name1", "value1"), 
                new Metadatum("name2", "value2"), 
                new Metadatum("name1", "value2") 
            };
            var sut = StreamChangeBuilder.Default.WithMetadata(metadata).Build();
            var result = sut.Metadata;
            Assert.That(result, Is.EquivalentTo(metadata));
        }

        [Test]
        public void VerifyEquality()
        {
            var messageId1 = Guid.NewGuid();
            var messageId2 = Guid.NewGuid();
            var message1 = new object();
            var message2 = new object();
            var metadata1 = new[]
            {
                new Metadatum("name1", "value1"), 
                new Metadatum("name2", "value2"), 
                new Metadatum("name1", "value2") 
            };

            var metadata2 = new[]
            {
                new Metadatum("name3", "value3"), 
                new Metadatum("name4", "value4"), 
                new Metadatum("name3", "value4") 
            };
            new EqualityAssertion(new StreamChange(messageId1, message1, metadata1)).
                VerifyEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId1, message1, metadata1)).
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new object()).
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId2, message1, metadata1)).
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId1, message2, metadata1)).
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId1, message1, metadata2)).
                Assert();
        }

        class StreamChangeBuilder
        {
            public static readonly StreamChangeBuilder Default = new StreamChangeBuilder(
                Guid.Empty, new object(), new Metadatum[0]);

            private readonly Guid _messageId;
            private readonly object _message;
            private readonly Metadatum[] _metadata;

            StreamChangeBuilder(Guid messageId, object message, Metadatum[] metadata)
            {
                _messageId = messageId;
                _message = message;
                _metadata = metadata;
            }

            public StreamChangeBuilder WithMessageId(Guid value)
            {
                return new StreamChangeBuilder(value, _message, _metadata);
            }

            public StreamChangeBuilder WithMessage(object value)
            {
                return new StreamChangeBuilder(_messageId, value, _metadata);
            }

            public StreamChangeBuilder WithMetadata(Metadatum[] value)
            {
                return new StreamChangeBuilder(_messageId, _message, value);
            }

            public StreamChange Build()
            {
                return new StreamChange(
                    _messageId,
                    _message,
                    _metadata);
            }
        }
    }
}
