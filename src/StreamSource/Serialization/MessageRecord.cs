using System;

namespace StreamSource.Serialization
{
    public class MessageRecord
    {
        public readonly string Contract;
        public readonly byte[] Data;

        public MessageRecord(string contract, byte[] data)
        {
            if (contract == null) throw new ArgumentNullException("contract");
            if (data == null) throw new ArgumentNullException("data");
            Contract = contract;
            Data = data;
        }
    }
}