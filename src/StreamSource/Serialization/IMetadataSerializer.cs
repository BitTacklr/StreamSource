namespace StreamSource.Serialization
{
    public interface IMetadataSerializer
    {
        byte[] Deserialize(Metadatum[] metadata);
    }
}