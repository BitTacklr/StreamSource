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
            Assert.Throws<ArgumentNullException>(() => 
                MetadatumBuilder.Default.WithName(null).Build());
        }

        [Test]
        public void ValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MetadatumBuilder.Default.WithValue(null).Build());
        }

        [Test]
        public void NameReturnsExpectedValue()
        {
            var name = Guid.NewGuid().ToString();
            var sut = MetadatumBuilder.Default.WithName(name).Build();
            var result = sut.Name;
            Assert.That(result, Is.EqualTo(name));
        }

        [Test]
        public void ValueReturnsExpectedValue()
        {
            var value = Guid.NewGuid().ToString();
            var sut = MetadatumBuilder.Default.WithValue(value).Build();
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
                VerifyNotEqual(new Metadatum("", ""), new object()).
                VerifyNotEqual(new Metadatum(name1, value1), new Metadatum(name2, value1)).
                VerifyNotEqual(new Metadatum(name1, value1), new Metadatum(name1, value2)).
                Assert();
        }

        class MetadatumBuilder
        {
            public static readonly MetadatumBuilder Default = 
                new MetadatumBuilder("", "");

            private readonly string _name;
            private readonly string _value;

            MetadatumBuilder(string name, string value)
            {
                _name = name;
                _value = value;
            }

            public MetadatumBuilder WithName(string value)
            {
                return new MetadatumBuilder(value, _value);
            }

            public MetadatumBuilder WithValue(string value)
            {
                return new MetadatumBuilder(_name, value);
            }

            public Metadatum Build()
            {
                return new Metadatum(_name, _value);
            }
        }
    }
}
