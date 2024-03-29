﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Data;
using NSE.WebAPI.Core.Configuration;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Middlewares;

namespace NSE.Catalogo.API.Configurations;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandlingConfiguration();
        services.AddCompressionConfiguration();
        
        services.AddDbContext<CatalogContext>(options
            => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
                        .AllowAnyHeader()
                        .AllowAnyMethod());
        });

        return services;
    }

    public static WebApplication UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
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

        app.MapControllers();
        
        return app;
    }
}
