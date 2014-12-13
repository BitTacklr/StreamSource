using System;
using StreamSource.Naming;

namespace StreamSource
{
    public class StreamChangeset
    {
        public readonly StreamName Name;
        public readonly int ExpectedVersion;
        public readonly Guid CausationId;
        public readonly Guid CorrelationId;
        public readonly StreamChange[] Changes;

        public StreamChangeset(StreamName name, int expectedVersion, Guid causationId, Guid correlationId, StreamChange[] changes)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (expectedVersion < 0) throw new ArgumentOutOfRangeException("expectedVersion", expectedVersion, "The expected version must be greater than or equal to 0.");
            if (changes == null) throw new ArgumentNullException("changes");
            Name = name;
            ExpectedVersion = expectedVersion;
            CausationId = causationId;
            CorrelationId = correlationId;
            Changes = changes;
        }
    }
}