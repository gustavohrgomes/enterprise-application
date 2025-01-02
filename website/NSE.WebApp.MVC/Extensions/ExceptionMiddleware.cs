using NSE.WebApp.MVC.Exceptions;
using NSE.WebApp.MVC.Services;
using Polly.CircuitBreaker;
using Refit;
using System.Net;
using System.Security.Authentication;

namespace NSE.WebApp.MVC.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private static IAutenticacaoService _autenticacaoService;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;

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
        catch (AuthenticationException)
        {
            HandleRequestExceptionAsync(context, HttpStatusCode.Unauthorized);
        }
    }

    private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            if (_autenticacaoService.TokenExpirado())
            {
                if (_autenticacaoService.RefreshTokenValido().Result)
                {
                    context.Response.Redirect(context.Request.Path);
                    return;
                }
            }

            _autenticacaoService.RealizarLogout();
            context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
            return;
        }

        context.Response.StatusCode = (int)statusCode;
    }

    private static void HandleBrokenCircuitExceptionAsync(HttpContext context)
    {
        try
        {
            context.Response.Redirect("/sistema-indisponivel");
        }
        catch
        {
            throw;
        }
    }
}
