using System;
using System.Text;

namespace StreamSource.Naming
{
    public class StreamNameComponent : IEquatable<StreamNameComponent>
    {
        private readonly string _name;
        private readonly string _value;

        public StreamNameComponent(string name, string value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
        }

        public bool Equals(StreamNameComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            return string.Equals(_name, other._name) && string.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StreamNameComponent && Equals((StreamNameComponent) obj);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _value.GetHashCode();
        }

        public static bool operator ==(StreamNameComponent left, StreamNameComponent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StreamNameComponent left, StreamNameComponent right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Name.Length + Value.Length + 1);
            foreach (var @char in Name)
            {
                if (StreamNameFormat.Escapable(@char))
                {
                    builder.Append(StreamNameFormat.Escape);
                }
                builder.Append(@char);
            }
            builder.Append(StreamNameFormat.ComponentNameValueSeparator);
            foreach (var @char in Value)
            {
                if (StreamNameFormat.Escapable(@char))
                {
                    builder.Append(StreamNameFormat.Escape);
                }
                builder.Append(@char);
            }
            return builder.ToString();
        }
    }
}