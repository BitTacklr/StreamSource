using System;
using NUnit.Framework;
using StreamSource.Framework;

namespace StreamSource.Naming
{
    [TestFixture]
    public class StreamNameComponentTests
    {
        [Test]
        public void NameCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactoryWithName(null));
        }

        [Test]
        public void ValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactoryWithValue(null));
        }

        [Test]
        public void NameReturnsExpectedResult()
        {
            var sut = SutFactoryWithName("name");
            Assert.That(sut.Name, Is.EqualTo("name"));
        }

        [Test]
        public void ValueReturnsExpectedResult()
        {
            var sut = SutFactoryWithValue("value");
            Assert.That(sut.Value, Is.EqualTo("value"));
        }

        [Test]
        public void VerifyEquality()
        {
            new EqualityAssertion(SutFactory()).
                VerifyEqual(SutFactory("name", "value"), SutFactory("name", "value")).
                VerifyNotEqual(SutFactory("name", "value1"), SutFactory("name", "value2")).
                VerifyNotEqual(SutFactory("name1", "value"), SutFactory("name2", "value")).
                Assert();
        }

        [TestCase("name", "value", "name=value")]
        [TestCase("name\\", "value", "name\\\\=value")]
        [TestCase("name=", "value", "name\\==value")]
        [TestCase("name,", "value", "name\\,=value")]
        [TestCase("name", "value\\", "name=value\\\\")]
        [TestCase("name", "value=", "name=value\\=")]
        [TestCase("name", "value,", "name=value\\,")]
        public void ToStringReturnsExpectedResult(string name, string value, string expected)
        {
            var sut = SutFactory(name, value);
            var result = sut.ToString();
            Assert.That(result, Is.EqualTo(expected));
        }

        private static StreamNameComponent SutFactoryWithName(string name)
        {
            return SutFactory(name, Guid.NewGuid().ToString("N"));
        }

        private static StreamNameComponent SutFactoryWithValue(string value)
        {
            return SutFactory(Guid.NewGuid().ToString("N"), value);
        }

        private static StreamNameComponent SutFactory()
        {
            return SutFactory(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
        }

        private static StreamNameComponent SutFactory(string name, string value)
        {
            return new StreamNameComponent(name, value);
        }
    }
}