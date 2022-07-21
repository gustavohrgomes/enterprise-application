using MediatR;
using NSE.Clientes.API.Configurations;
using NSE.WebAPI.Core.Identidade;

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

services.AddApiconfiguration(builder.Configuration);
services.AddJwtConfiguration(builder.Configuration);
services.AddSwaggerConfiguration();
services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseApiConfiguration();

app.Run();
