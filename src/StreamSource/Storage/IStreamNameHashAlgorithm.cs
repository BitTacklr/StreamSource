using StreamSource.Naming;

namespace StreamSource.Storage
{
    public interface IStreamNameHashAlgorithm
    {
        byte[] ComputeHash(StreamName name);
    }
}