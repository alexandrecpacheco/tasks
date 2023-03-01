using AutoMapper;
using Serilog;
using System.Reflection;
using Tasks.Domain.AutoMapper;
using Tasks.Domain.Events;
using Tasks.Domain.Interfaces.Data;
using Tasks.Handler.EventHandlers;
using Tasks.IoC;
using Tasks.IoC.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDependencyInjection();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddLogging();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddMediatR(c => c.TypeEvaluator.Invoke(typeof(TaskCreatedEventHandler)));
//builder.Services.AddMediatR(c => AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddMediatR(c => Assembly.GetExecutingAssembly());

var app = builder.Build();

app.Services.GetRequiredService<IDatabase>().UpgradeIfNecessary();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(o =>
    {
        o.AllowAnyHeader();
        o.AllowAnyMethod();
        o.AllowAnyOrigin();
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRabbitMq().SubscribeEvent<TaskCreatedEvent>();

app.Run();

RegisterMappings();

CreateHostBuilder(args).Build().Run();

Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", $"Task RabbitMq Consumer - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}")
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();

static MapperConfiguration RegisterMappings()
{
    return new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new DomainToResponseMappingProfile());
        cfg.AddProfile(new RequestToDomainMapperTask());
    });
}

static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Program>();
                });

