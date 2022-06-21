using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NSE.Identidade.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
{
    services.AddIdentityconfiguration(builder.Configuration);

    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c => 
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "NerdStore Enterprise Identity API",
            Description = "Esta API faz parte do curso de ASP.NET Core Enterprise Applications.",
            Contact = new OpenApiContact() { Name = "Gustavo Gomes", Email = "dev.gustavogomes@gmail.com" },
            License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        }));
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c 
            => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
    }

    //app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}