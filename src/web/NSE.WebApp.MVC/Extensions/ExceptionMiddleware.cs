using NSE.WebApp.MVC.Exceptions;
using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NSE.WebApp.MVC.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomHttpRequestException ex)
        {
            HandleRequestExceptionAsync(context, ex.StatusCode);
        }
        catch (ValidationApiException ex)
        {
            HandleRequestExceptionAsync(context, ex.StatusCode);
        }
        catch (ApiException ex)
        {
            HandleRequestExceptionAsync(context, ex.StatusCode);
        }
        catch (BrokenCircuitException)
        {
            HandleBrokenCircuitExceptionAsync(context);
        }
    }

    private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
            return;
        }

        context.Response.StatusCode = (int)statusCode;
    }

    private static void HandleBrokenCircuitExceptionAsync(HttpContext context) => context.Response.Redirect("/sistema-indisponível");
}
