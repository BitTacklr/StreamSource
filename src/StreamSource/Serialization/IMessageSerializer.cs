namespace StreamSource.Serialization
{
    public interface IMessageSerializer
    {
        MessageRecord Serialize(object message);
    }
}