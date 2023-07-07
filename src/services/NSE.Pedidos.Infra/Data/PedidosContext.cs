using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NSE.Core.Communication;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Messages;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.Infra.Data;

public class PedidosContext : DbContext
{
    private readonly IMediatorHandler _mediatorHandler;

    public PedidosContext(DbContextOptions<PedidosContext> options, IMediatorHandler mediatorHandler)
        : base(options)
    {
        _mediatorHandler = mediatorHandler;
    }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(250);

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<ValidationResult>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.HasSequence<int>("NumeroPedidos").StartsAt(1000).IncrementsBy(1);

        base.OnModelCreating(modelBuilder);
    }
}

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var aggregates = ctx.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = aggregates
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var aggregateEntry in aggregates)
        {
            aggregateEntry.Entity.ClearDomainEvents();
        }

        var tasks = domainEvents.Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));

        await Task.WhenAll(tasks);
    }
}

// public class PedidosContextDesignTimeFactory : IDesignTimeDbContextFactory<PedidosContext>
// {
//     public PedidosContext CreateDbContext(string[] args)
//     {
//         var configurationBuilder = new ConfigurationBuilder();
//
//         var dotnetEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
//         var aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//
//         var appsettingsPath = Directory.GetDirectoryRoot("/src/services/NSE.Pedidos.API/");
//
//         configurationBuilder
//             .SetBasePath(appsettingsPath)
//             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//
//         if (!string.IsNullOrWhiteSpace(dotnetEnvironment))
//         {
//             configurationBuilder.AddJsonFile($"appsettings.{dotnetEnvironment}.json", true, true);
//         }
//         
//         if (!string.IsNullOrWhiteSpace(aspnetcoreEnvironment))
//         {
//             configurationBuilder.AddJsonFile($"appsettings.{aspnetcoreEnvironment}.json", true, true);
//         }
//
//         var configuration = configurationBuilder.AddEnvironmentVariables().Build();
//
//         var connectionString = configuration.GetConnectionString("DefaultConnection");
//
//         if (string.IsNullOrWhiteSpace(connectionString))
//         {
//             throw new NullReferenceException("Connection string not found.");
//         }
//         
//         var builder = new DbContextOptionsBuilder<PedidosContext>().UseSqlServer(connectionString!);
//
//         return new PedidosContext(builder.Options, null);
//     }
// }