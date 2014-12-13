using StreamSource.Naming;

namespace StreamSource
{
    public interface IStreamStoreWriterSelector
    {
        IStreamStoreWriter Select(StreamName name);
    }
}