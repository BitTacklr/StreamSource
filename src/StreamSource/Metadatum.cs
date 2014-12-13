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
    }
}