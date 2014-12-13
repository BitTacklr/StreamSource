using System;

namespace StreamSource
{
    public class Metadatum
    {
        public readonly string Name;
        public readonly string Value;

        public Metadatum(string name, string value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            Name = name;
            Value = value;
        }

        public bool Equals(Metadatum other)
        {
            if (ReferenceEquals(other, null)) return false;
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Metadatum)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Value.GetHashCode();
        }

        public static bool operator ==(Metadatum left, Metadatum right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Metadatum left, Metadatum right)
        {
            return !Equals(left, right);
        }
    }
}