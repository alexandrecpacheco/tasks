using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.Interfaces.Messaging;
using Tasks.Infraestructure.RabbitMq;
using Tasks.Infraestructure.Utils;

namespace Tasks.IoC.Messaging
{
    [ExcludeFromCodeCoverage]
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
