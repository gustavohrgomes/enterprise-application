using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;

public class HttpForbiddenResponse : HttpResponse
{
    public HttpForbiddenResponse()
        : base(HttpStatusCode.Forbidden)
    { }
}
