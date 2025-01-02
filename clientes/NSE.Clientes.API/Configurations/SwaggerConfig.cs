using Microsoft.OpenApi.Models;

namespace NSE.Clientes.API.Configurations;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "NerdStore Enterprise Clientes API",
                Description = "Esta API faz parte do curso ASP.NET Core Enterprise Applications.",
                Contact = new OpenApiContact() { Name = "Gustavo Gomes", Email = "dev.gustavogomes@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });

            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Insira o token JWT desta maneira: Bearer <seu token>",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
            });
        });

        return services;
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c
            => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
    }
}
