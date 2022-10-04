using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Identidade.API.Models;
using NSE.MessageBus;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NSE.Identidade.API.Controllers;

[Route("api/identidade")]
public class AuthController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMessageBus _bus;
    private readonly IJwtService _jwtService;

    public AuthController(SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          IConfiguration configuration,
                          IMessageBus bus,
                          IJwtService jwtService)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
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

        var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

        if (result.Succeeded)
        {
            var clienteResult = await RegistrarCliente(usuarioRegistro);
            
            if (!clienteResult.ValidationResult.IsValid)
            {
                await _userManager.DeleteAsync(user);
                return CustomResponse(clienteResult.ValidationResult);
            }
            
            return CustomResponse(await GerarJwtToken(usuarioRegistro.Email));
        }

        foreach (var error in result.Errors)
            AdicionarErroProcessamento(error.Description);

        return CustomResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var user = await _userManager.FindByEmailAsync(usuarioLogin.Email);

        if (user is null)
        {
            AdicionarErroProcessamento("Usuário ou senha incorretos");
            return CustomResponse();
        };

        var result = await _signInManager.PasswordSignInAsync(user, usuarioLogin.Senha, false, true);

        if (result.Succeeded) return CustomResponse(await GerarJwtToken(usuarioLogin.Email));

        if (result.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            return CustomResponse();
        }

        AdicionarErroProcessamento("Usuário ou senha incorretos");
        return CustomResponse();
    }

    private async Task<UsuarioRespostaLogin> GerarJwtToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);

        var identityClaims = await ObterClaimsUsuario(user, claims);
        var encodedToken = await CodificarToken(identityClaims);
        
        return ObterRespostaToken(user, claims, encodedToken);
    }

    private async Task<string> CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var currentIssuer = $"{ControllerContext.HttpContext.Request.Scheme}://{ControllerContext.HttpContext.Request.Host}";
        
        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = currentIssuer,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(IdentityUser user, IList<Claim> claims)
    {
        var roles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString()));

        foreach (var role in roles)
            claims.Add(new Claim("role", role));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private UsuarioRespostaLogin ObterRespostaToken(IdentityUser user, IList<Claim> claims, string encodedToken)
    {
        return new UsuarioRespostaLogin
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_configuration.GetValue<int>("AppSettings:ExpiracaoHoras")).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch
        {
            await _userManager.DeleteAsync(usuario);
            throw;
        }
    }
}

