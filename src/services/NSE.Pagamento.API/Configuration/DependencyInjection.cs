using NSE.Pagamentos.API.Data;
using NSE.Pagamentos.API.Data.Repository;
using NSE.Pagamentos.API.Models;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pagamentos.API.Configuration;

public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<PagamentosContext>();
    }
}
