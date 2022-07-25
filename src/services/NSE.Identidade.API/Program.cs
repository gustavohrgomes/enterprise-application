using NSE.Identidade.API.Configurations;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;

var services = builder.Services;

builder.Configuration
    .SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsetting.{hostEnvironment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

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