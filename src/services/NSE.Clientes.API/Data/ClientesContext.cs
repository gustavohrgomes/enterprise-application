using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Communication;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Data;

public sealed class ClientesContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler) 
        : base(options)
    {

        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        _mediatorHandler = mediatorHandler;
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

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

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync()
    {
        var sucesso = await base.SaveChangesAsync() > 0;
        if (sucesso) _mediatorHandler.PublicarEventos(context: this);

        return sucesso;
    }
}

public static class MediatorExtensions
{
    public static async void PublicarEventos<T>(this IMediatorHandler mediator, T context) where T : DbContext
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Eventos is not null && x.Entity.Eventos.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Eventos)
            .ToList();

        domainEntities
            .ToList()
            .ForEach(x => x.Entity.LimparEventos());

        var tasks = domainEvents
            .Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));

        await Task.WhenAll(tasks);
    }
}