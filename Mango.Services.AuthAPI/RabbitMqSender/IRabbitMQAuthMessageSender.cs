namespace Mango.Services.AuthAPI.RabbitMqSender
{
    public interface IRabbitMQAuthMessageSender
    {
        Task SendMessage(object message, string queueName);
    }
}
