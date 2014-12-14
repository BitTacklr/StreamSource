using System;
using NUnit.Framework;
using StreamSource.Framework;

namespace StreamSource.Serialization
{
    [TestFixture]
    public class MessageRecordTests
    {
        [Test]
        public void ContractCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MessageRecordBuilder.Default.WithContract(null).Build());
        }

        [Test]
        public void DataCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MessageRecordBuilder.Default.WithData(null).Build());
        }

        [Test]
        public void ContractReturnsExpectedData()
        {
            var contract = Guid.NewGuid().ToString();
            var sut = MessageRecordBuilder.Default.WithContract(contract).Build();
            var result = sut.Contract;
            Assert.That(result, Is.EqualTo(contract));
        }

        [Test]
        public void DataReturnsExpectedData()
        {
            var data = new byte[] { 1, 2, 3 };
            var sut = MessageRecordBuilder.Default.WithData(data).Build();
            var result = sut.Data;
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void VerifyEquality()
        {
            var contract1 = Guid.NewGuid().ToString();
            var contract2 = Guid.NewGuid().ToString();
            var data1 = new byte[] { 1, 2, 3};
            var data2 = new byte[] { 4, 5, 6};
            var data3 = new byte[] {1, 2, 3, 4};
            new EqualityAssertion(new MessageRecord("", new byte[0])).
                VerifyEqual(new MessageRecord(contract1, data1), new MessageRecord(contract1, data1)).
                VerifyNotEqual(new MessageRecord(contract1, data1), new object()).
                VerifyNotEqual(new MessageRecord(contract1, data1), new MessageRecord(contract2, data1)).
                VerifyNotEqual(new MessageRecord(contract1, data1), new MessageRecord(contract1, data2)).
                VerifyNotEqual(new MessageRecord(contract1, data1), new MessageRecord(contract1, data3)).
                Assert();
        }

        class MessageRecordBuilder
        {
            public static readonly MessageRecordBuilder Default =
                new MessageRecordBuilder("", new byte[0]);

            private readonly string _contract;
            private readonly byte[] _data;

            MessageRecordBuilder(string contract, byte[] data)
            {
                _contract = contract;
                _data = data;
            }

            public MessageRecordBuilder WithContract(string value)
            {
                return new MessageRecordBuilder(value, _data);
            }

            public MessageRecordBuilder WithData(byte[] value)
            {
                return new MessageRecordBuilder(_contract, value);
            }

            public MessageRecord Build()
            {
                return new MessageRecord(_contract, _data);
            }
        }
    }
}
