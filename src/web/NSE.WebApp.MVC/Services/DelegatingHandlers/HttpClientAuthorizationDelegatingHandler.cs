﻿using NSE.WebApp.MVC.Extensions;
using System.Net.Http.Headers;

namespace NSE.WebApp.MVC.Services.DelegatingHandlers;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IUser _user;

    public HttpClientAuthorizationDelegatingHandler(IUser user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = _user.ObterHttpContext().Response.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authorizationHeader))
            request.Headers.Add("Authorization", new string[] { authorizationHeader });

        var token = _user.ObterUserToken();

        if (token is not null) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}
