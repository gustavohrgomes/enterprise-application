using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Pagamentos.API.Data;
using NSE.Pagamentos.API.Facade;
using NSE.WebAPI.Core.Configuration;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Middlewares;

namespace NSE.Pagamentos.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandlingConfiguration();
        services.AddCompressionConfiguration();
        
        services.AddDbContext<PagamentosContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

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
        
        app.UseResponseCompression();
        
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
