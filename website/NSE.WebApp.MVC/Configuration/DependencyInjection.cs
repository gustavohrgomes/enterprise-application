using Microsoft.AspNetCore.Mvc.DataAnnotations;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.DelegatingHandlers;

namespace NSE.WebApp.MVC.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        #region HttpServices

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>(config =>
            config.BaseAddress = new Uri(configuration.GetValue<string>("AutenticacaoUrl")))
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        services.AddHttpClient<ICatalogoService, CatalogService>(config =>
            config.BaseAddress = new Uri(configuration.GetValue<string>("CatalogoUrl")))
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        services.AddHttpClient<IComprasBffService, ComprasBffService>(config =>
            config.BaseAddress = new Uri(configuration.GetValue<string>("ComprasBffUrl")))
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        services.AddHttpClient<IClienteService, ClienteService>(config =>
            config.BaseAddress = new Uri(configuration.GetValue<string>("ClienteUrl")))
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AllowSelfSignedCertificate()
            .AddStandardResilienceHandler();

        #endregion

        return services;
    }
}
