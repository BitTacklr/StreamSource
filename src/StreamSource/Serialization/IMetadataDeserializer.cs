namespace StreamSource.Serialization
{
    public interface IMetadataDeserializer
    {
        Metadatum[] Deserialize(byte[] data);
    }
}