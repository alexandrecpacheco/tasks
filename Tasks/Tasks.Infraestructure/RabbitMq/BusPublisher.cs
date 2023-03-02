using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Tasks.Domain.Events;
using Tasks.Domain.Messaging;
using Tasks.Infraestructure.Utils;

namespace Tasks.Infraestructure.RabbitMq
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IConfiguration _configuration;

        public BusPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendProductMessage<T>(TaskCreatedEvent message)
        {
            var config = new ConfigurationTask(_configuration).GetTaskConfiguration();

            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = config.Password
            };

            var connection = factory.CreateConnection();

            using
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: config.ExchangeName, type: ExchangeType.Fanout);
            channel.QueueDeclare(config.QueueName, exclusive: false);
            channel.QueueBind(config.QueueName, config.ExchangeName, config.BindName);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: config.ExchangeName, routingKey: "", body: body, basicProperties: null);
        }
    }
}
