using MassTransit;
using NSE.Core.Logging;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Identidade.API.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;

builder.Configuration
    .SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

//builder.Host.UseSerilog((contextBuilder, loggerConfiguration) => loggerConfiguration.Configure(contextBuilder));

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services
    .AddApiConfiguration()
    .AddIdentityconfiguration(builder.Configuration)
    .AddRabbitMQMessagingConfiguration(builder.Configuration)
    .AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseApiConfiguration();
app.Run();