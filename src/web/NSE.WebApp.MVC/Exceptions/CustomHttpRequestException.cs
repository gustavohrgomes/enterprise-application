using System.Net;

namespace NSE.WebApp.MVC.Exceptions;

public class CustomHttpRequestException : Exception
{
    public HttpStatusCode StatusCode;

    public CustomHttpRequestException(HttpStatusCode statusCode)
        : this(statusCode, null, null) 
    { }

    public CustomHttpRequestException(HttpStatusCode statusCode, string message) 
        : this(statusCode, message, null)
    { }

    public CustomHttpRequestException(HttpStatusCode statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}
