using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Usuario;

public class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public Guid ObterUserId()
    {
        var estaAutenticado = EstaAutenticado();

        var idDoUsuario = Guid.Parse(_accessor.HttpContext.User.GetUserId());

        return estaAutenticado ? idDoUsuario : Guid.Empty;
    }

    public string ObterUserEmail() => EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : "";

    public string ObterUserToken() => EstaAutenticado() ? _accessor.HttpContext.User.GetUserToken() : "";

    public string ObterUserRefreshToken() => EstaAutenticado() ? _accessor.HttpContext.User.GetUserRefreshToken() : "";

    public bool EstaAutenticado() => _accessor.HttpContext.User.Identity.IsAuthenticated;

    public bool PossuiRole(string role) => _accessor.HttpContext.User.IsInRole(role);

    public IEnumerable<Claim> ObterClaims() => _accessor.HttpContext.User.Claims;

    public HttpContext ObterHttpContext() => _accessor.HttpContext;
}

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var claim = principal.FindFirst("email");
        return claim?.Value;
    }

    public static string GetUserToken(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var claim = principal.FindFirst("JWT");
        return claim?.Value;
    }

    public static string GetUserRefreshToken(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var claim = principal.FindFirst("RefreshToken");
        return claim?.Value;
    }
}
