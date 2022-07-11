using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.DelegatingHandlers;
using Polly;
using Polly.Extensions.Http;

namespace NSE.WebApp.MVC.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>(config => 
            config.BaseAddress = new Uri(configuration.GetValue<string>("AutenticacaoUrl")));

        services.AddHttpClient<ICatalogoService, CatalogService>(config =>
            config.BaseAddress = new Uri(configuration.GetValue<string>("CatalogoUrl")))
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUser, AspNetUser>();

        #region Refit
        //services.AddHttpClient("Refit", config =>
        //    config.BaseAddress = new Uri(configuration.GetValue<string>("CatalogoUrl")))
        //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
        //    .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
        #endregion Refit

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        // In this case will wait for
        //  2 ^ 1 = 2 seconds then
        //  2 ^ 2 = 4 seconds then
        //  2 ^ 3 = 8 seconds then
        //  2 ^ 4 = 16 seconds then
        //  2 ^ 5 = 32 seconds

        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Tentando pela {retryCount} vez!");
                        Console.ForegroundColor = ConsoleColor.White;
                    });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30));
    }
}
