using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Services;
using NSE.WebAPI.Core.Configuration;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.HttpResponses;
using NSE.WebAPI.Core.Middlewares;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Identidade.API.Configurations;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddExceptionHandlingConfiguration();
        services.AddCompressionConfiguration();
        
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.GetErrors();

                var errorResponse = new HttpBadRequestResponse(errors);

                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();
        
        if (app.Environment.IsEnvironment("Testing"))
        {
            using var scope = app.Services.CreateScope();
            var identidadeContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            identidadeContext.Database.Migrate();
        }

        app.UseRouting();

        app.UseAuthConfiguration();
        
        app.UseResponseCompression();
        
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.MapControllers();

        app.UseJwksDiscovery();
    }
}
