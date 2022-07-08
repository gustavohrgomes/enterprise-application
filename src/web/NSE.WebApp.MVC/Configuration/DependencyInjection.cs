using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>(config => 
            config.BaseAddress = new Uri(configuration.GetValue<string>("AutenticacaoUrl")));

        services.AddHttpClient<ICatalogoService, CatalogService>(config => 
            config.BaseAddress = new Uri(configuration.GetValue<string>("CatalogoUrl")));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUser, AspNetUser>();

        return services;
    }
}
