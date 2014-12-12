using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StreamSource.Framework;

namespace StreamSource.Naming
{
    [TestFixture]
    public class StreamNameTests
    {
        [Test]
        public void ComponentsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory(null));
        }

        [Test]
        public void EmptyReturnsExpectedResult()
        {
            var sut = StreamName.Empty;
            Assert.That(sut.Components, Is.EquivalentTo(new StreamNameComponent[0]));
            Assert.That(sut.ToString(), Is.EqualTo(""));
        }

        [Test]
        public void ComponentsReturnsExpectedResult()
        {
            var components = new[]
            {
                new StreamNameComponent("name1", "value1"),
                new StreamNameComponent("name2", "value2")
            };
            var sut = SutFactory(components);
            var result = sut.Components;
            Assert.That(result, Is.EquivalentTo(components));
        }

        [Test]
        public void VerifyEquality()
        {
            var components1 = new[]
            {
                new StreamNameComponent("name1", "value1"),
                new StreamNameComponent("name2", "value2")
            };
            var components2 = new[]
            {
                new StreamNameComponent("name1", "value1"),
                new StreamNameComponent("name2", "value2")
            };
            var components3 = new[]
            {
                new StreamNameComponent("name1", "value1")
            };
            var components4 = new[]
            {
                new StreamNameComponent("name3", "value3"),
                new StreamNameComponent("name4", "value4")
            };
            new EqualityAssertion(SutFactory(new StreamNameComponent[0])).
                VerifyEqual(SutFactory(components1), SutFactory(components1)).
                VerifyEqual(SutFactory(components1), SutFactory(components2)).
                VerifyNotEqual(SutFactory(components1), SutFactory(components3)).
                VerifyNotEqual(SutFactory(components1), SutFactory(components4)).
                Assert();
        }

        [Test]
        public void CanBeImplicitlyCastToString()
        {
            var sut = SutFactory();

            string result = sut;

            Assert.That(result, Is.EqualTo(sut.ToString()));
        }

        [Test]
        public void CanBeExplicitlyCastToString()
        {
            var sut = SutFactory();

            var result = (string)sut;

            Assert.That(result, Is.EqualTo(sut.ToString()));
        }

        [Test]
        public void AppendComponentCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Append(null));
        }

        [Test]
        public void AppendReturnsExpectedResult()
        {
            var sut = SutFactory();
            var components = sut.Components;
            var appendComponent = new StreamNameComponent(
                Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"));
            var result = sut.Append(appendComponent);
            Assert.That(result, Is.EqualTo(SutFactory(components.Concat(new[]
            {
                appendComponent
            }).ToArray())));
        }

        [Test]
        public void PrependComponentCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Prepend(null));
        }

        [Test]
        public void PrependReturnsExpectedResult()
        {
            var sut = SutFactory();
            var components = sut.Components;
            var prependComponent = new StreamNameComponent(
                Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"));
            var result = sut.Prepend(prependComponent);
            Assert.That(result, Is.EqualTo(SutFactory(new[]
            {
                prependComponent
            }.Concat(components).ToArray())));
        }

        [TestCaseSource("ToStringCases")]
        public void ToStringReturnsExpectedResult(StreamNameComponent[] components, string expected)
        {
            var sut = SutFactory(components);
            var result = sut.ToString();
            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> ToStringCases()
        {
            //Empty
            yield return new TestCaseData(
                new StreamNameComponent[0], "");
            //Single component
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name", "value")
                }, "name=value");
            //Multi component
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name", "value"),
                    new StreamNameComponent("name", "value")
                }, "name=value,name=value");
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name1", "value1"),
                    new StreamNameComponent("name2", "value2")
                }, "name1=value1,name2=value2");
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name2", "value2"),
                    new StreamNameComponent("name1", "value1")
                }, "name2=value2,name1=value1");
            //Escaping smoke tests
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name\\", "value\\"),
                    new StreamNameComponent("name=", "value="),
                    new StreamNameComponent("name,", "value,")
                }, "name\\\\=value\\\\,name\\==value\\=,name\\,=value\\,");
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("\\name", "\\value"),
                    new StreamNameComponent("=name", "=value"),
                    new StreamNameComponent(",name", ",value")
                }, "\\\\name=\\\\value,\\=name=\\=value,\\,name=\\,value");
        }

        [Test]
        public void IndexerNameCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => { var _ = SutFactory()[null]; });
        }

        [TestCaseSource("IndexerCases")]
        public void IndexerReturnsExpectedComponent(StreamNameComponent[] input, string name, StreamNameComponent[] expected)
        {
            var sut = SutFactory(input);
            var result = sut[name];
            Assert.That(result, Is.EquivalentTo(expected));
        }

        private static IEnumerable<TestCaseData> IndexerCases()
        {
            yield return new TestCaseData(
                new StreamNameComponent[0],
                "name",
                new StreamNameComponent[0]);
            yield return new TestCaseData(
                new []
                {
                    new StreamNameComponent("name1", "value")
                },
                "name",
                new StreamNameComponent[0]);
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name", "value")
                },
                "name",
                new []
                {
                    new StreamNameComponent("name", "value")
                });
            yield return new TestCaseData(
                new[]
                {
                    new StreamNameComponent("name", "value1"),
                    new StreamNameComponent("name", "value2")
                },
                "name",
                new[]
                {
                    new StreamNameComponent("name", "value1"),
                    new StreamNameComponent("name", "value2")
                });
        }

        private static StreamName SutFactory(StreamNameComponent[] components)
        {
            return new StreamName(components);
        }

        private static StreamName SutFactory()
        {
            return SutFactory(new[]
            {
                new StreamNameComponent("name", "value"), 
            });
        }
    }
}