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
                new StreamChange(Guid.Empty, null, new Metadatum[0]));
        }

        [Test]
        public void MetadataCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new StreamChange(Guid.Empty, new object(), null));
        }

        [Test]
        public void MetadataCanBeEmpty()
        {
            Assert.DoesNotThrow(() =>
                new StreamChange(Guid.Empty, new object(), new Metadatum[0]));
        }

        [Test]
        public void MessageIdReturnsExpectedResult()
        {
            var messageId = Guid.NewGuid();
            var sut = new StreamChange(messageId, new object(), new Metadatum[0]);
            var result = sut.MessageId;
            Assert.That(result, Is.EqualTo(messageId));
        }

        [Test]
        public void MessageReturnsExpectedResult()
        {
            var message = new object();
            var sut = new StreamChange(Guid.Empty, message, new Metadatum[0]);
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
            var sut = new StreamChange(Guid.Empty, new object(), metadata);
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
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId2, message1, metadata1)).
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId1, message2, metadata1)).
                VerifyNotEqual(new StreamChange(messageId1, message1, metadata1), new StreamChange(messageId1, message1, metadata2)).
                Assert();
        }
    }
}
