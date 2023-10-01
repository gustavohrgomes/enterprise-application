using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NSE.Core.Communication;
using NSE.Core.DomainObjects;

namespace NSE.Core.Data;

public class UnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly IPublisher _publisher;

    public UnitOfWork(TDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<bool> CommitAsync()
    {
        UpdateAuditableEntities();
        return await _context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false) > 0;
    }

    public async Task<bool> ResilientCommitAsync(CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        var result = await strategy.ExecuteAsync(async () => await TryExecuteStrategy(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false))
            .ConfigureAwait(continueOnCapturedContext: false);

        return result;
    }

    private async Task<bool> TryExecuteStrategy(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var domainEvents = ExtractDomainEventsFromAggregates();

            var result = await CommitAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            await _context.Database.CommitTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var dispatchingTasks = domainEvents.Select(x => _publisher.Publish(x, cancellationToken));

            await Task.WhenAll(dispatchingTasks);

            return result;
        }
        catch
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }
    
    public IEnumerable<INotification> ExtractDomainEventsFromAggregates()
    {
        var aggregates = _context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();
        
        var domainEvents = aggregates.SelectMany(x => x.Entity.DomainEvents).ToList();
        
        foreach (var aggregateEntry in aggregates)
        {
            aggregateEntry.Entity.ClearDomainEvents();
        }

        return domainEvents;
    }
    
    private void UpdateAuditableEntities()
    {
        foreach (var entry in _context.ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }
    }
}