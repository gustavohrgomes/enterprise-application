﻿using Microsoft.AspNetCore.Http;

namespace NSE.Core.Communication;

public class ResponseResult
{
    public ResponseResult()
    {
        Errors = new List<string>();
    }

    public string Title { get; set; }
    public int StatusCode { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public static ResponseResult Ok() => new() { StatusCode = StatusCodes.Status200OK };

    public static ResponseResult BadRequest(IEnumerable<string> errors) =>
        new() { StatusCode = StatusCodes.Status400BadRequest, Errors = errors };
}

public class ResponseResult<T> : ResponseResult
{
    public T Result { get; set; }

    public ResponseResult(T result)
    {
        Result = result;
    }
}

public class ResponseErrorMessages
{
    public ResponseErrorMessages()
    {
        Mensagens = new List<string>();
    }

    public List<string> Mensagens { get; set; }
}
