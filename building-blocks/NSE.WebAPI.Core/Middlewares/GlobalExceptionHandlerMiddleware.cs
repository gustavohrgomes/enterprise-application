using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSE.WebAPI.Core.Handlers;

namespace NSE.WebAPI.Core.Middlewares;

public sealed class GlobalExceptionHandlerMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IExceptionHandler _exceptionHandler;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, 
                                            IExceptionHandler exceptionHandler, 
                                            RequestDelegate next)
    {
        _logger = logger;
        _exceptionHandler = exceptionHandler;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception has occurred while executing '{MiddlewareName}.{MethodName}()' method.", 
                nameof(GlobalExceptionHandlerMiddleware), nameof(Invoke));

            await _exceptionHandler.HandleExceptionAsync(httpContext, ex);
        }
    }
}