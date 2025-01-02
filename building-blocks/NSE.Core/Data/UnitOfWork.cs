using MediatR;
using Microsoft.EntityFrameworkCore;
using NSE.Core.DomainObjects;

namespace NSE.Core.Data;

public class UnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public UnitOfWork(TDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CommitAsync()
    {
        UpdateAuditableEntities();
        return await _context.SaveChangesAsync() > 0;
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

   
}