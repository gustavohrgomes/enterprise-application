﻿using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repositories;
using NSE.Clientes.API.Models;
using NSE.Core.Communication;
using NSE.Core.Data;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.API.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<ClientesContext>>();

        return services;
    }
}
