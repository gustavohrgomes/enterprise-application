using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repositories;
using NSE.Clientes.API.Models;
using NSE.Clientes.API.Services;
using NSE.Core.Communication;

namespace NSE.Clientes.API.Configurations;

public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
    }
}
