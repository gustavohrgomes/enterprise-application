using NSE.Core.Logging;
using NSE.Identidade.API.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;

var services = builder.Services;

builder.Configuration
    .SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((contextBuilder, loggerConfiguration) => loggerConfiguration.Configure(contextBuilder));

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

services.AddApiConfiguration();
services.AddIdentityconfiguration(builder.Configuration);
services.AddSwaggerConfiguration();
services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerDocumentation();

app.UseApiConfiguration();

app.Run();