﻿using NSE.WebAPI.Core.Identidade;

namespace NSE.Identidade.API.Configurations;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseRouting();

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
