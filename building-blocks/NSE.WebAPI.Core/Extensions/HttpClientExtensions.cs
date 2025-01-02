using Microsoft.Extensions.DependencyInjection;

namespace NSE.WebAPI.Core.Extensions;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder AllowSelfSignedCertificate(this IHttpClientBuilder client)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));

        return client.ConfigureHttpMessageHandlerBuilder(builder =>
        {
            builder.PrimaryHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });
    }
}
