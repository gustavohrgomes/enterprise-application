using System.Net;

namespace NSE.WebAPI.Core.HttpResponses;
public class HttpCreatedResponse : HttpResponse
{
    public HttpCreatedResponse()
        : base(HttpStatusCode.Created)
    { }
}

public class HttpCreatedResponse<T> : HttpCreatedResponse
{
    public T Result { get; }

    public HttpCreatedResponse(T result)
    {
        Result = result;
    }
}
