using System;
using NUnit.Framework;
using StreamSource.Framework;

namespace StreamSource
{
    [TestFixture]
    public class MetadatumTests
    {
        [Test]
        public void NameCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Metadatum(null, ""));
        }

        [Test]
        public void ValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Metadatum("", null));
        }

        [Test]
        public void NameReturnsExpectedValue()
        {
            var name = Guid.NewGuid().ToString();
            var sut = new Metadatum(name, "");
            var result = sut.Name;
            Assert.That(result, Is.EqualTo(name));
        }

        [Test]
        public void ValueReturnsExpectedValue()
        {
            var value = Guid.NewGuid().ToString();
            var sut = new Metadatum("", value);
            var result = sut.Value;
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void VerifyEquality()
        {
            var name1 = Guid.NewGuid().ToString();
            var name2 = Guid.NewGuid().ToString();
            var value1 = Guid.NewGuid().ToString();
            var value2 = Guid.NewGuid().ToString();
            new EqualityAssertion(new Metadatum("", "")).
                VerifyEqual(new Metadatum("", ""), new Metadatum("", "")).
                VerifyEqual(new Metadatum(name1, value1), new Metadatum(name1, value1)).
                VerifyNotEqual(new Metadatum(name1, value1), new Metadatum(name2, value1)).
                VerifyNotEqual(new Metadatum(name1, value1), new Metadatum(name1, value2)).
                Assert();
        }
    }
}
