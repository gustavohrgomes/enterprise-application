using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;

public class HttpOkResponse : HttpResponse
{
    public HttpOkResponse()
        : base(HttpStatusCode.OK)
    { }
}

public class HttpOkResponse<T> : HttpOkResponse
{
    public T Result { get; set; }

    public HttpOkResponse(T result)
    {
        Result = result;
    }
}
