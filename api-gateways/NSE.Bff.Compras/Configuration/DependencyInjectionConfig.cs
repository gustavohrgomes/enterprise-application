using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.Usuario;
using Polly;

namespace NSE.Bff.Compras.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<ICatalogoService, CatalogoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        services.AddHttpClient<ICarrinhoService, CarrinhoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        services.AddHttpClient<IPedidoService, PedidoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        services.AddHttpClient<IClienteService, ClienteService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();
    }
}