using Microsoft.OpenApi.Models;

namespace NSE.Catalogo.API.Configurations;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NerdStore Enterprise Catálogo API",
                Description = "Esta API faz parte do curso de ASP.NET Core Enterprise Applications.",
                Contact = new OpenApiContact() { Name = "Gustavo Gomes", Email = "dev.gustavogomes@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            }));
    }

    public static void UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c
            => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
    }
}
