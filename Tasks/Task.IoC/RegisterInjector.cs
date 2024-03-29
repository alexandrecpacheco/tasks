﻿using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.Interfaces.Data;
using Tasks.Domain.Interfaces.Data.Repository;
using Tasks.Domain.Interfaces.Data.Service;
using Tasks.Infraestructure;
using Tasks.Infraestructure.Data;
using Tasks.Services;

namespace Tasks.IoC
{
    [ExcludeFromCodeCoverage]
    public static class RegisterInjector
    {
        public static void RegisterDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IDatabase, Database>();

            services.AddScoped<ITaskService, TaskService>();

            services.AddScoped<ITaskRepository, TaskRepository>();
        }
    }
}
