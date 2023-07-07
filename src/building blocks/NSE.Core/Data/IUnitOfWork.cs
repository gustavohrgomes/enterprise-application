using MediatR;

namespace NSE.Core.Data;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
    IEnumerable<INotification> ExtractDomainEventsFromAggregates();
}
