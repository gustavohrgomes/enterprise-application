using Microsoft.AspNetCore.Http;

namespace NSE.WebAPI.Core.Handlers;

public interface IExceptionHandler
{
    Task HandleExceptionAsync(HttpContext httpContext, 
                              Exception exception,
                              CancellationToken cancellationToken = default!);
}