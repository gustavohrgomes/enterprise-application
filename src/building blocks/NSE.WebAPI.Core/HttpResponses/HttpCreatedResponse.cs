using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
