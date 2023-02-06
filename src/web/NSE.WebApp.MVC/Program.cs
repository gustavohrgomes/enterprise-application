using NSE.Core.Logging;
using NSE.WebApp.MVC.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
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

services.AddIdentityConfiguration();
services.AddWebAppConfiguration();
services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.UseWebAppConfiguration();

app.Run();
