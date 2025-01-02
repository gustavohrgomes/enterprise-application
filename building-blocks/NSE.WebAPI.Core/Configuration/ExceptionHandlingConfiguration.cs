using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSE.WebAPI.Core.Handlers;

namespace NSE.WebAPI.Core.Configuration;

public static class ExceptionHandlingConfiguration
{
    public static void AddExceptionHandlingConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(nameof(services));
        
        services.TryAddTransient<IExceptionHandler, ExceptionHandler>();
    }
}