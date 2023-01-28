using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;
public class HttpConflictResponse : HttpResponse
{
    public string[] Errors { get; }

    public HttpConflictResponse(params string[] errors)
         : base(HttpStatusCode.Conflict)
    {
        if (errors?.Any() ?? false)
        {
            Errors = errors;
        }
    }
}
