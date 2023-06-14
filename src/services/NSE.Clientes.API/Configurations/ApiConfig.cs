﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Data;
using NSE.WebAPI.Core.Configuration;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.HttpResponses;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Clientes.API.Configurations;

public static class ApiConfig
{
    public static void AddApiconfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClientesContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        services.AddCompressionConfiguration();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                policy =>
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());
        });
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
