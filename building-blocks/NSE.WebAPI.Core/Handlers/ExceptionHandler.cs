using Microsoft.AspNetCore.Http;
using NSE.WebAPI.Core.HttpResponses;
using System.Net.Mime;
using System.Text.Json;
using HttpResponse = NSE.WebAPI.Core.HttpResponses.HttpResponse;

namespace NSE.WebAPI.Core.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async Task HandleExceptionAsync(HttpContext httpContext,
                                           Exception exception,
                                           CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(httpContext));
        ArgumentNullException.ThrowIfNull(nameof(exception));

        var response = FromHttpContext(httpContext, exception);

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = response.HttpStatusCode.GetHashCode();

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, response.GetType(), jsonOptions);

        await httpContext.Response.WriteAsync(json, cancellationToken);
    }

    private static HttpResponse FromHttpContext(HttpContext httpContext, Exception exception)
    {
        var errors = string.IsNullOrWhiteSpace(exception.Message)
            ? Array.Empty<string>()
            : new[] { exception.Message };

        HttpResponse response = httpContext.Response.StatusCode switch {
            StatusCodes.Status400BadRequest => new HttpBadRequestResponse(errors),
            StatusCodes.Status409Conflict => new HttpConflictResponse(errors),
            StatusCodes.Status403Forbidden => new HttpForbiddenResponse(),
            StatusCodes.Status404NotFound => new HttpNotFoundResponse(errors),
            StatusCodes.Status401Unauthorized => new HttpUnauthorizedAccessResponse(errors),
            StatusCodes.Status500InternalServerError => new HttpInternalServerErrorResponse(errors),
            _ => new HttpInternalServerErrorResponse(errors)
        };

        return response;
    }
}