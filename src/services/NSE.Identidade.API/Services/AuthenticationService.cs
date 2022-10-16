using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Models;
using NSE.WebAPI.Core.Usuario;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.Identidade.API.Services;

public interface IAuthenticationService
{
    SignInManager<IdentityUser> SignInManager { get; }
    UserManager<IdentityUser> UserManager { get; }
    Task<UsuarioRespostaLogin> GerarJwtToken(string email);
    Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;
    private readonly IAspNetUser _aspNetUser;

    public AuthenticationService(SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager,
                                 ApplicationDbContext context,
                                 IConfiguration configuration,
                                 IJwtService jwtService,
                                 IAspNetUser aspNetUser)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        _aspNetUser = aspNetUser ?? throw new ArgumentNullException(nameof(aspNetUser));
    }

    public SignInManager<IdentityUser> SignInManager => _signInManager;
    public UserManager<IdentityUser> UserManager => _userManager;

    public async Task<UsuarioRespostaLogin> GerarJwtToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);

        var identityClaims = await ObterClaimsUsuario(user, claims);
        var encodedToken = await CodificarToken(identityClaims);

        var refreshToken = await GerarRefreshToken(email);

        return ObterRespostaToken(user, claims, encodedToken, refreshToken);
    }

    public async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
    {
        var token = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        return token != null && token.ExpirationDate.ToLocalTime() > DateTimeOffset.Now
            ? token
            : null;
    }

    private async Task<string> CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var currentIssuer = $"{_aspNetUser.ObterHttpContext().Request.Scheme}://{_aspNetUser.ObterHttpContext().Request.Host}";

        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = currentIssuer,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddSeconds(30),
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

    private UsuarioRespostaLogin ObterRespostaToken(IdentityUser user, IList<Claim> claims, string encodedToken, RefreshToken refreshToken)
    {
        return new UsuarioRespostaLogin
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private async Task<RefreshToken> GerarRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            Username = email,
            ExpirationDate = DateTime.UtcNow.AddHours(_configuration.GetValue<double>("AppTokenSettings:RefreshTokenExpiration"))
        };

        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(rt => rt.Username == email));
        
        await _context.RefreshTokens.AddAsync(refreshToken);

        await _context.SaveChangesAsync();

        return refreshToken;
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
