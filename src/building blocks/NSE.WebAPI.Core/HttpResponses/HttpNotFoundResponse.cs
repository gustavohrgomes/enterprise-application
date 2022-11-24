using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;

public class HttpNotFoundResponse : HttpResponse
{
    public string[] Errors { get; }

    public HttpNotFoundResponse(params string[] errors)
        : base(HttpStatusCode.NotFound)
    {
        if (errors?.Any() ?? false)
        {
            Errors = errors;
        }
    }
}
