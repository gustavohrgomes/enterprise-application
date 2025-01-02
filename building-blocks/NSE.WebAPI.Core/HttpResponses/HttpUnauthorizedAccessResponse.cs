using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;
public class HttpUnauthorizedAccessResponse : HttpResponse
{
    public string[] Errors { get; }

    public HttpUnauthorizedAccessResponse(params string[] errors)
        : base(HttpStatusCode.Unauthorized)
    {
        if (errors?.Any() ?? false)
        {
            Errors = errors;
        }
    }
}
