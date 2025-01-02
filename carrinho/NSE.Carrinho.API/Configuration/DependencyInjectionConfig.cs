﻿using NSE.Carrinho.API.Data;
using NSE.Core.Data;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<IUnitOfWork, UnitOfWork<CarrinhoContext>>();

        return services;
    }
}