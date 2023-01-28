using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;
public abstract class HttpResponse
{
    public HttpStatusCode HttpStatusCode { get; }

    public HttpResponse(HttpStatusCode httpStatusCode)
    {
        HttpStatusCode = httpStatusCode;
    }
}
