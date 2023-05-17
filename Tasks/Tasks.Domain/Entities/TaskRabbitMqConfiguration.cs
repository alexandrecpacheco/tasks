using System.Diagnostics.CodeAnalysis;

namespace Tasks.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TaskRabbitMqConfiguration
    {
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
        public string? BindName { get; set; }
        public string? ExchangeName { get; set; }
        public string? QueueName { get; set; }
    }
}
