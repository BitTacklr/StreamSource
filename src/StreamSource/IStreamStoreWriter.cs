namespace StreamSource
{
    public interface IStreamStoreWriter
    {
        void Write(StreamChangeset changeset);
    }
}
