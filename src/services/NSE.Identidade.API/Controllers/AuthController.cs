using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Identidade.API.Models;
using NSE.Identidade.API.Services;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.HttpResponses;

namespace NSE.Identidade.API.Controllers;

[Route("api/identidade")]
public class AuthController : MainController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthController> _logger;
    private readonly IRequestClient<UsuarioRegistradoIntegrationEvent> _requestClient;

    public AuthController(IAuthenticationService authenticationService,
                          ILogger<AuthController> logger,
                          IRequestClient<UsuarioRegistradoIntegrationEvent> requestClient)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService)); ;
        _logger = logger;
        _requestClient = requestClient;
    }

    [HttpPost("nova-conta")]
    [ProducesResponseType(typeof(HttpCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(HttpBadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(HttpInternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
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
                return HttpBadRequest(clienteResult.ValidationResult);
            }
            
            return HttpCreated(await _authenticationService.GerarJwtToken(usuarioRegistro.Email));
        }

        foreach (var error in result.Errors)
            AdicionarErroProcessamento(error.Description);

        return HttpBadRequest();
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(HttpOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpBadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(HttpInternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        var user = await _authenticationService.UserManager.FindByEmailAsync(usuarioLogin.Email);

        if (user is null)
        {
            AdicionarErroProcessamento("Usuário ou senha incorretos");
            return HttpBadRequest();
        };

        var result = await _authenticationService.SignInManager.PasswordSignInAsync(user, usuarioLogin.Senha, false, true);

        if (result.Succeeded) return HttpOk(await _authenticationService.GerarJwtToken(usuarioLogin.Email));

        if (result.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            return HttpBadRequest();
        }

        AdicionarErroProcessamento("Usuário ou senha incorretos");
        return HttpBadRequest();
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(HttpOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpBadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(HttpInternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            AdicionarErroProcessamento("Refresh Token inválido");
            return HttpBadRequest();
        }

        var token = await _authenticationService.ObterRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            AdicionarErroProcessamento("Refresh Token expirado");
            return HttpBadRequest();
        }

        return HttpOk(await _authenticationService.GerarJwtToken(token.Username));
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await _authenticationService.UserManager.FindByEmailAsync(usuarioRegistro.Email);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            //return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);

            var response = await _requestClient.GetResponse<ResponseMessage>(usuarioRegistrado);

            return response.Message;
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao registrar o cliente. {0}", ex.Message);
            await _authenticationService.UserManager.DeleteAsync(usuario);
            throw;
        }
    }
}

