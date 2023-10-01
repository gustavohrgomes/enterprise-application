using MediatR.NotificationPublishers;
using NSE.Catalogo.API.Configurations;
using NSE.Core.Logging;
using NSE.WebAPI.Core.Identidade;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;

builder.Configuration
    .SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsetting.{hostEnvironment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((contextBuilder, loggerConfiguration) => loggerConfiguration.Configure(contextBuilder));

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services
    .AddApiConfiguration(builder.Configuration)
    .AddJwtConfiguration(builder.Configuration)
    .AddSwaggerConfiguration()
    .AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblyContaining<Program>();
        config.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .RegisterServices()
    .AddRabbitMQMessagingConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseSerilogRequestLogging();

app.UseApiConfiguration();

app.Run();

public partial class Program { }