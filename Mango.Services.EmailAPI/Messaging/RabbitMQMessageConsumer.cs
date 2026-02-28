using Mango.Services.EmailAPI.Models.Dto;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Data.Common;
using System.Text;
using System.Threading.Channels;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQMessageConsumer: IMessageConsumer
    {
        private readonly string _messageBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQMessageConsumer(IOptions<MessageQueueSettings> options, IConfiguration configuration)
        {
            _configuration = configuration;
            _messageBusConnectionString = options.Value.RabbitMQ.ConnectionString;

            /* TODO: Here GetValue was not working somehow
             * _emailCartQueue = _configuration.GetValue<string>("Key");
             */
            _emailCartQueue = configuration["TopicAndQueueNames:EmailShoppingCartQueue"];

            //_registerUserQueue =
            //    configuration["TopicAndQueueNames:RegisterUserQueue"];

            //_orderCreatedQueue =
            //    configuration["TopicAndQueueNames:OrderCreatedTopic"];

            // TODO: Refactor it. Not satisfied with this arrangements. The reason is - if async is available,
            // with this setting it remains unused.
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_messageBusConnectionString)
            };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;

            _channel.QueueDeclareAsync(_emailCartQueue, false, false, false, null);
            //_channel.QueueDeclare(_registerUserQueue, false, false, false, null);
            //_channel.QueueDeclare(_orderCreatedQueue, false, false, false, null);
        }

        private async Task StartEmailCartConsumer()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var objMessage =
                    JsonConvert.DeserializeObject<CartDto>(message);

                try
                {
                    //await _emailService.EmailCartAndLog(objMessage);

                    _channel.BasicAckAsync(args.DeliveryTag, false);
                }
                catch
                {
                    // optionally requeue
                    await _channel.BasicNackAsync(args.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _emailCartQueue,
                autoAck: false,
                consumer: consumer);
        }

        public async Task Start()
        {
            await StartEmailCartConsumer();
            //await StartRegisterUserConsumer();
            //await StartOrderPlacedConsumer();
        }

        public async Task Stop()
        {
            await _channel.DisposeAsync();
            await _connection.DisposeAsync();
        }

        //Pass error handler from RabbitMQ documentation
        private Task ErrorHandler()
        {
            Console.WriteLine("");
            return Task.CompletedTask;
        }

        //private async Task StartRegisterUserConsumer()
        //{
        //    var consumer = new EventingBasicConsumer(_channel);

        //    consumer.Received += async (sender, args) =>
        //    {
        //        var body = args.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);

        //        string email =
        //            JsonConvert.DeserializeObject<string>(message);

        //        try
        //        {
        //            await _emailService.RegisterUserEmailAndLog(email);
        //            _channel.BasicAck(args.DeliveryTag, false);
        //        }
        //        catch
        //        {
        //            await _channel.BasicNack(args.DeliveryTag, false, true);
        //        }
        //    };

        //    await _channel.BasicConsume(
        //        queue: _registerUserQueue,
        //        autoAck: false,
        //        consumer: consumer);
        //}

        //private async Task StartOrderPlacedConsumer()
        //{
        //    var consumer = new EventingBasicConsumer(_channel);

        //    consumer.Received += async (sender, args) =>
        //    {
        //        var body = args.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);

        //        RewardsMessage objMessage =
        //            JsonConvert.DeserializeObject<RewardsMessage>(message);

        //        try
        //        {
        //            await _emailService.LogOrderPlaced(objMessage);
        //            _channel.BasicAck(args.DeliveryTag, false);
        //        }
        //        catch
        //        {
        //           await _channel.BasicNack(args.DeliveryTag, false, true);
        //        }
        //    };

        //    await _channel.BasicConsume(
        //        queue: _orderCreatedQueue,
        //        autoAck: false,
        //        consumer: consumer);
        //}
    }
}
