﻿using NSE.Core.Communication;
using NSE.Core.Data;
using NSE.Pedidos.API.Application.Queries;
using NSE.Pedidos.API.Services;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Domain.Vouchers;
using NSE.Pedidos.Infra.Data;
using NSE.Pedidos.Infra.Data.Repository;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pedidos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // API
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IVoucherQueries, VoucherQueries>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();

        // Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<PedidosContext>>();

        services.AddHostedService<PedidoOrquestradorIntegrationHandler>();
    }
}