using MediatR;
using NSE.Clientes.API.Configurations;
using NSE.Core.Logging;
using NSE.WebAPI.Core.Identidade;
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

services.AddApiconfiguration(builder.Configuration);
services.AddJwtConfiguration(builder.Configuration);
services.AddSwaggerConfiguration();
services.AddMediatR(config 
    => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.RegisterServices(builder.Configuration);
services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseApiConfiguration();

app.Run();

public partial class Program { }
