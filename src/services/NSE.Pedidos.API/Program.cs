using MediatR;
using NSE.Core.Logging;
using NSE.Pedidos.API.Configuration;
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

builder.Host.UseSerilog((contextBuilder, loggerConfiguration) => loggerConfiguration.Configure(contextBuilder.Configuration));

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

services.AddApiConfiguration(builder.Configuration);
services.AddJwtConfiguration(builder.Configuration);
services.AddSwaggerConfiguration();
services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
services.RegisterServices();
services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseApiConfiguration(builder.Environment);

app.Run();