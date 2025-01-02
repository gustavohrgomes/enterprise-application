using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;
public class HttpBadRequestResponse : HttpResponse
{
    public string[] Errors { get; }

    public HttpBadRequestResponse(params string[] errors)
         : base(HttpStatusCode.BadRequest)
    {
        if (errors?.Any() ?? false)
        {
            Errors = errors;
        }
    }
}
