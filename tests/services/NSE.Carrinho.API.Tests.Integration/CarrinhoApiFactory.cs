using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSE.Carrinho.API.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Testcontainers.MsSql;
using Xunit;

namespace NSE.Carrinho.API.Tests.Integration;

public class CarrinhoApiFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private const string _defaultScheme = "Test";
    private const string _symmetricTestingKey = "integration-testing-security-key";

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    public HttpClient HttpClient { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder loggingBuilder) =>
        {
            loggingBuilder.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<CarrinhoContext>))!);

            services.RemoveAll(typeof(IHostedService));
            services.RemoveAll(typeof(DbContext));

            services.AddDbContext<CarrinhoContext>(options
                => options.UseSqlServer(_dbContainer.GetConnectionString()));

            services.AddSingleton<CarrinhoContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = _defaultScheme;
                options.DefaultChallengeScheme = _defaultScheme;
                options.DefaultScheme = _defaultScheme;
            }).AddJwtBearer(_defaultScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_symmetricTestingKey)),
                };
            });
        });

        builder.UseEnvironment("Testing");

        base.ConfigureWebHost(builder);
    }

    private void LoginToApi() 
        => HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateAccessToken());

    private static string GenerateAccessToken()
    {
        var identityClaims = new ClaimsIdentity();

        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()));
        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, new Faker().Internet.Email().ToLower()));

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddMinutes(20),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_symmetricTestingKey)), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        HttpClient = CreateClient();
        LoginToApi();
    }

    public async new Task DisposeAsync()
        => await _dbContainer.DisposeAsync();
}
