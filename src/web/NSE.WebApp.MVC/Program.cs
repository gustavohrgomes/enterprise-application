using NSE.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddIdentityConfiguration();
services.AddWebAppConfiguration();

var app = builder.Build();

app.UseWebAppConfiguration();

app.Run();
