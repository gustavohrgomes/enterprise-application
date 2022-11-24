using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;
public class HttpInternalServerErrorResponse : HttpResponse
{
    public string[] Errors { get; }

    public HttpInternalServerErrorResponse(params string[] errors)
       : base(HttpStatusCode.InternalServerError)
    {
        if (errors?.Any() ?? false)
        {
            Errors = errors;
        }
    }
}