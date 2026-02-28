namespace Mango.MessageBus
{
    public interface IMessageConsumer
    {
        Task Start();
        Task Stop();
    }
}