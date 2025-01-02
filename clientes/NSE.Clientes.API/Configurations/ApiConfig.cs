using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Data;
using NSE.WebAPI.Core.Configuration;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Middlewares;

namespace NSE.Clientes.API.Configurations;

public static class ApiConfig
{
    public static IServiceCollection AddApiconfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandlingConfiguration();
        services.AddCompressionConfiguration();
        
        services.AddDbContext<ClientesContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                policy =>
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());
        });

        return services;
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();
        
        if (app.Environment.IsEnvironment("Testing"))
        {
            using var scope = app.Services.CreateScope();
            var clientesContext = scope.ServiceProvider.GetRequiredService<ClientesContext>();
            clientesContext.Database.Migrate();
        }

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.UseResponseCompression();
        
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        
        app.MapControllers();
    }
}
