using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NSE.Core.Communication;
using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Services;

public interface IAutenticacaoService
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
    Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro);
    Task RealizarLogin(UsuarioRespostaLogin resposta);
    Task RealizarLogout();
    bool TokenExpirado();
    Task<bool> RefreshTokenValido();
}

public class AutenticacaoService : Service, IAutenticacaoService
{
    private readonly HttpClient _httpClient;

    private readonly IAuthenticationService _authenticationService;
    private readonly IAspNetUser _user;

    public AutenticacaoService(HttpClient httpClient, 
                               IAuthenticationService authenticationService, 
                               IAspNetUser user)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
    {
        var loginContent = ObterConteudo(usuarioLogin);

        var response = await _httpClient.PostAsync("/api/identidade/login", loginContent);

        if (!TratarErrosResponse(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro)
    {
        var registroContent = ObterConteudo(usuarioRegistro);

        var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);

        if (!TratarErrosResponse(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
    {
        var refreshTokenContent = ObterConteudo(refreshToken);

        var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

        if (!TratarErrosResponse(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task RealizarLogin(UsuarioRespostaLogin resposta)
    {
        var token = ObterTokenFormatado(resposta.AccessToken);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", resposta.AccessToken));
        claims.Add(new Claim("RefreshToken", resposta.RefreshToken));
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = true,
        };

        await _authenticationService.SignInAsync(_user.ObterHttpContext(),
                                                 CookieAuthenticationDefaults.AuthenticationScheme,
                                                 new ClaimsPrincipal(claimsIdentity),
                                                 authProperties);
    }

    public async Task RealizarLogout()
        => await _authenticationService.SignOutAsync(_user.ObterHttpContext(),
                                                     CookieAuthenticationDefaults.AuthenticationScheme,
                                                     null);

    public async Task<bool> RefreshTokenValido()
    {
        var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

        if (resposta.AccessToken is not null && resposta.ResponseResult is null)
        {
            await RealizarLogin(resposta);
            return true;
        }

        return false;
    }

    public static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        => new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;

    public bool TokenExpirado()
    {
        var jwt = _user.ObterUserToken();
        if (jwt is null) return false;

        var token = ObterTokenFormatado(jwt);
        return token.ValidTo.ToLocalTime() < DateTime.Now;
    }
}
