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

    [ValidateAntiForgeryToken]
    [HttpPost("nova-conta")]
    public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return View(usuarioRegistro);

        var resposta = await _autenticacaoService.Registrar(usuarioRegistro);

        if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

        await _autenticacaoService.RealizarLogin(resposta);

        return RedirectToAction("Index", "Catalogo");
    }

    [HttpGet("login")]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost("login")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(usuarioLogin);

        var response = await _autenticacaoService.Login(usuarioLogin);

        if (ResponsePossuiErros(response.ResponseResult)) return View(usuarioLogin);

        await _autenticacaoService.RealizarLogin(response);

        if (string.IsNullOrWhiteSpace(returnUrl)) return RedirectToAction("Index", "");

        return LocalRedirect(returnUrl);
    }
        
    [HttpGet("sair")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Catalogo");
    }
}
