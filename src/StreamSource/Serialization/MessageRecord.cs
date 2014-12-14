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

        public bool Equals(MessageRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            return string.Equals(Contract, other.Contract) && Data.Equals(other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MessageRecord)obj);
        }

        public override int GetHashCode()
        {
            return Contract.GetHashCode() ^ Data.GetHashCode();
        }

        public static bool operator ==(MessageRecord left, MessageRecord right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MessageRecord left, MessageRecord right)
        {
            return !Equals(left, right);
        }
    }
}