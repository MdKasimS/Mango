using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(object message, string topic_queue_name)
        {
            //await using var client = new ServiceBusClient();
        }

        public Task SendMessage(object message, string queueName)
        {
            throw new NotImplementedException();
        }
    }
}
