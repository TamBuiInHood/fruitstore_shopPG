using Contracts.Common.Interfaces;
using Contracts.Messages;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly ISerializeServices _serializeServices;

        public RabbitMQProducer(ISerializeServices serializeServices)
        {
            _serializeServices = serializeServices;
        }

        public void SendMessage<T>(T message)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            var connection =  connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("orders", exclusive: false);

            var jsonData = _serializeServices.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonData);

            channel.BasicPublish(exchange: "", routingKey: "orders", body: body);
        }
    }
}
