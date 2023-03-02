using AutoMapper;
using Tasks.Domain.AutoMapper;
using Tasks.Domain.Interfaces.Data;
using Tasks.IoC;
using Tasks.IoC.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDependencyInjection();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddLogging();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.Run();

RegisterMappings();

static MapperConfiguration RegisterMappings()
{
    return new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new DomainToResponseMappingProfile());
        cfg.AddProfile(new RequestToDomainMapperTask());
    });
}


