using NSE.Catalogo.API.Configurations;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;
var services = builder.Services;

services.AddApiConfiguration(builder.Configuration);
services.AddSwaggerConfiguration();
services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseApiConfiguration();

app.Run();
