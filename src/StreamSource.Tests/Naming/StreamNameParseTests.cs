using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace StreamSource.Naming
{
    [TestFixture]
    public class StreamNameParseTests
    {
        [Test]
        public void ParseValueCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => StreamName.Parse(null));
        }

        [TestCaseSource("MalformedParseCases")]
        public void ParseThrows(string value)
        {
            Assert.Throws<FormatException>(() => StreamName.Parse(value));
        }

        private static IEnumerable<TestCaseData> MalformedParseCases()
        {
            //Cannot be empty
            yield return new TestCaseData("");
            //Cannot start with a reserved character
            yield return new TestCaseData("\\");
            yield return new TestCaseData(",");
            yield return new TestCaseData("=");
            //Name not specified
            yield return new TestCaseData("=value");
            yield return new TestCaseData("=value,name=value");
            yield return new TestCaseData("name=value,=value");
            //Value not specified
            yield return new TestCaseData("name=");
            yield return new TestCaseData("name=,name=value");
            yield return new TestCaseData("name=value,name=");
            //Name not properly escaped
            yield return new TestCaseData("\\=value");
            yield return new TestCaseData("\\name=value");
            yield return new TestCaseData("==value");
            yield return new TestCaseData("=name=value");
            yield return new TestCaseData(",=value");
            yield return new TestCaseData(",name=value");
            yield return new TestCaseData("\\=value,name=value");
            yield return new TestCaseData("\\name=value,name=value");
            yield return new TestCaseData("==value,name=value");
            yield return new TestCaseData("=name=value,name=value");
            yield return new TestCaseData(",=value,name=value");
            yield return new TestCaseData(",name=value,name=value");
            yield return new TestCaseData("name=value,\\=value");
            yield return new TestCaseData("name=value,\\name=value");
            yield return new TestCaseData("name=value,==value");
            yield return new TestCaseData("name=value,=name=value");
            yield return new TestCaseData("name=value,,=value");
            yield return new TestCaseData("name=value,,name=value");
            //Value not properly escaped
            yield return new TestCaseData("name=\\");
            yield return new TestCaseData("name=\\value");
            yield return new TestCaseData("name==");
            yield return new TestCaseData("name==value");
            yield return new TestCaseData("name=,");
            yield return new TestCaseData("name=,value");
            yield return new TestCaseData("name=\\,name=value");
            yield return new TestCaseData("name=\\value,name=value");
            yield return new TestCaseData("name==,name=value");
            yield return new TestCaseData("name==value,name=value");
            yield return new TestCaseData("name=,,name=value");
            yield return new TestCaseData("name=,value,name=value");
            yield return new TestCaseData("name=value,name=\\");
            yield return new TestCaseData("name=value,name=\\value");
            yield return new TestCaseData("name=value,name==");
            yield return new TestCaseData("name=value,name==value");
            yield return new TestCaseData("name=value,name=,");
            yield return new TestCaseData("name=value,name=,value");

            yield return new TestCaseData("name=value,");
            yield return new TestCaseData("name=value,,");
            yield return new TestCaseData("name=value,=,");
            yield return new TestCaseData("name=value\\");
        }

        [TestCaseSource("WellformedParseCases")]
        public void ParseReturnsExpectedResult(string value, StreamName name)
        {
            var result = StreamName.Parse(value);
            Assert.That(result, Is.EqualTo(name));
        }

        private static IEnumerable<TestCaseData> WellformedParseCases()
        {
            yield return new TestCaseData("name=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "value")
                }));
            yield return new TestCaseData("name=value,name=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "value"),
                    new StreamNameComponent("name", "value")
                }));
            yield return new TestCaseData("name=value1,name=value2", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "value1"),
                    new StreamNameComponent("name", "value2")
                }));
            yield return new TestCaseData("name1=value,name2=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name1", "value"),
                    new StreamNameComponent("name2", "value")
                }));

            //Name escaping
            yield return new TestCaseData("name\\\\=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name\\", "value")
                }));
            yield return new TestCaseData("name\\==value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name=", "value")
                }));
            yield return new TestCaseData("name\\,=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name,", "value")
                }));

            yield return new TestCaseData("\\\\name=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("\\name", "value")
                }));
            yield return new TestCaseData("\\=name=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("=name", "value")
                }));
            yield return new TestCaseData("\\,name=value", new StreamName(
                new[]
                {
                    new StreamNameComponent(",name", "value")
                }));

            yield return new TestCaseData("na\\\\me=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("na\\me", "value")
                }));
            yield return new TestCaseData("na\\=me=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("na=me", "value")
                }));
            yield return new TestCaseData("na\\,me=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("na,me", "value")
                }));

            //Value escaping
            yield return new TestCaseData("name=value\\\\", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "value\\")
                }));
            yield return new TestCaseData("name=value\\=", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "value=")
                }));
            yield return new TestCaseData("name=value\\,", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "value,")
                }));

            yield return new TestCaseData("name=\\\\value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "\\value")
                }));
            yield return new TestCaseData("name=\\=value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "=value")
                }));
            yield return new TestCaseData("name=\\,value", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", ",value")
                }));

            yield return new TestCaseData("name=val\\\\ue", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "val\\ue")
                }));
            yield return new TestCaseData("name=val\\=ue", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "val=ue")
                }));
            yield return new TestCaseData("name=val\\,ue", new StreamName(
                new[]
                {
                    new StreamNameComponent("name", "val,ue")
                }));
        }
    }
}