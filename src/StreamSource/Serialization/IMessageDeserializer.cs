namespace StreamSource.Serialization
{
    public interface IMessageDeserializer
    {
        object Deserialize(MessageRecord record);
    }
}