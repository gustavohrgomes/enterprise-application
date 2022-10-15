﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Identidade.API.Models;
using NSE.Identidade.API.Services;
using NSE.MessageBus;
using NSE.WebAPI.Core.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.Identidade.API.Controllers;

[Route("api/identidade")]
public class AuthController : MainController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    private readonly IMessageBus _bus;
    private readonly IJwtService _jwtService;

    public AuthController(IConfiguration configuration,
                          IMessageBus bus,
                          IJwtService jwtService,
                          IAuthenticationService authenticationService)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService)); ;
    }

    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var user = new IdentityUser
        {
            UserName = usuarioRegistro.Email,
            Email = usuarioRegistro.Email,
            EmailConfirmed = true
        };

        var result = await _authenticationService.UserManager.CreateAsync(user, usuarioRegistro.Senha);

        if (result.Succeeded)
        {
            var clienteResult = await RegistrarCliente(usuarioRegistro);
            
            if (!clienteResult.ValidationResult.IsValid)
            {
                await _authenticationService.UserManager.DeleteAsync(user);
                return CustomResponse(clienteResult.ValidationResult);
            }
            
            return CustomResponse(await _authenticationService.GerarJwtToken(usuarioRegistro.Email));
        }

        foreach (var error in result.Errors)
            AdicionarErroProcessamento(error.Description);

        return CustomResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var user = await _authenticationService.UserManager.FindByEmailAsync(usuarioLogin.Email);

        if (user is null)
        {
            AdicionarErroProcessamento("Usuário ou senha incorretos");
            return CustomResponse();
        };

        var result = await _authenticationService.SignInManager.PasswordSignInAsync(user, usuarioLogin.Senha, false, true);

        if (result.Succeeded) return CustomResponse(await _authenticationService.GerarJwtToken(usuarioLogin.Email));

        if (result.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            return CustomResponse();
        }

        AdicionarErroProcessamento("Usuário ou senha incorretos");
        return CustomResponse();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            AdicionarErroProcessamento("Refresh Token inválido");
            return CustomResponse();
        }

        var token = await _authenticationService.ObterRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            AdicionarErroProcessamento("Refresh Token expirado");
            return CustomResponse();
        }

        return CustomResponse(await _authenticationService.GerarJwtToken(token.Username));
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await _authenticationService.UserManager.FindByEmailAsync(usuarioRegistro.Email);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch
        {
            await _authenticationService.UserManager.DeleteAsync(usuario);
            throw;
        }
    }


}

