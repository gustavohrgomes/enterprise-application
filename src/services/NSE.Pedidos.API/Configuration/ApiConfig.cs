using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Pedidos.Infra.Data;
using NSE.WebAPI.Core.Configuration;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Middlewares;

namespace NSE.Pedidos.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandlingConfiguration();
        services.AddCompressionConfiguration();
        
        services.AddDbContext<PedidosContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
    }

    public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();
        
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}