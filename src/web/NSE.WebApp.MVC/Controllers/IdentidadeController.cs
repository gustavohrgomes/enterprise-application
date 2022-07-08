using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Controllers;

public class IdentidadeController : MainController
{
    private readonly IAutenticacaoService _autenticacaoService;

    public IdentidadeController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService ?? throw new ArgumentNullException(nameof(autenticacaoService));
    }

    [HttpGet("nova-conta")]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost("nova-conta")]
    public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return View(usuarioRegistro);

        var resposta = await _autenticacaoService.Registrar(usuarioRegistro);

        if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

        await RealizarLogin(resposta);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("login")]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(usuarioLogin);

        var response = await _autenticacaoService.Login(usuarioLogin);

        if (ResponsePossuiErros(response.ResponseResult)) return View(usuarioLogin);

        await RealizarLogin(response);

        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

        return LocalRedirect(returnUrl);
    }

    [HttpGet("sair")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task RealizarLogin(UsuarioRespostaLogin resposta)
    {
        var token = ObterTokenFormatado(resposta.AccessToken);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", resposta.AccessToken));
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var autoProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity),
            autoProperties);
    }

    private static JwtSecurityToken ObterTokenFormatado(string token)
        => new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
}
