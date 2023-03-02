using Microsoft.Extensions.Configuration;
using Tasks.Domain.Entities;

namespace Tasks.Infraestructure.Utils
{
    public class ConfigurationTask : IConfigurationTask
    {
        private readonly IConfiguration _configuration;
        public ConfigurationTask(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TaskRabbitMqConfiguration GetTaskConfiguration()
        {
            var hostName = _configuration.GetSection("RabbitMq:HostName").Value;
            var userName = _configuration.GetSection("RabbitMq:UserName").Value;
            var password = _configuration.GetSection("RabbitMq:Password").Value;
            var virtualHost = _configuration.GetSection("RabbitMq:VirtualHost").Value;
            var bind = _configuration.GetSection("RabbitMq:Bind:Name").Value;
            var exchange = _configuration.GetSection("RabbitMq:Exchange:Name").Value;
            var queue = _configuration.GetSection("RabbitMq:Queue:Name").Value;

            return new TaskRabbitMqConfiguration()
            {
                BindName = bind,
                ExchangeName = exchange,
                QueueName = queue,
                HostName = hostName,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost
            };
        }

    }
}
