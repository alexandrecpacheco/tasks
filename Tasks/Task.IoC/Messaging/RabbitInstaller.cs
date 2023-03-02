using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Domain.Messaging;
using Tasks.Infraestructure.RabbitMq;
using Tasks.Infraestructure.Utils;

namespace Tasks.IoC.Messaging
{
    public static class RabbitInstaller
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBusPublisher, BusPublisher>();
            services.AddScoped<IConfigurationTask, ConfigurationTask>();
            
            return services;
        }
    }
}
