using MediatR.NotificationPublishers;
using NSE.Core.Logging;
using NSE.Pagamentos.API.Configuration;
using NSE.WebAPI.Core.Identidade;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;

var services = builder.Services;

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

services
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
    app.UseSwaggerConfiguration();
}

app.UseApiConfiguration(builder.Environment);

app.Run();

public partial class Program { }