using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Identidade.API.Configurations;

public static class IdentityConfig
{
    public static void AddIdentityconfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddJwksManager()
            .PersistKeysToDatabaseStore<ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>(options
            => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<IdentityMensagensPortugues>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }
}

