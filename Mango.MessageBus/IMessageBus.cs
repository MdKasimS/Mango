using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.MessageBus
{
    internal interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_name);
    }
}
