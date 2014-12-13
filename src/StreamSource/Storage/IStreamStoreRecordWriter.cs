namespace StreamSource.Storage
{
    public interface IStreamStoreRecordWriter
    {
        void Write(StreamRecord[] records);
    }
}