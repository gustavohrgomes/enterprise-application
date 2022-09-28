using NSE.Bff.Compras.Configuration;
using NSE.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var hostEnvironment = builder.Environment;

builder.Configuration
    .SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsetting.{hostEnvironment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

services.AddApiConfiguration(builder.Configuration);
services.AddJwtConfiguration(builder.Configuration);
services.AddSwaggerConfiguration();
services.RegisterServices();
services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseApiConfiguration(builder.Environment);

app.Run();