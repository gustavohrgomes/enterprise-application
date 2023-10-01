using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace NSE.Core.Data;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
    Task<bool> ResilientCommitAsync(CancellationToken cancellationToken = default);
    IEnumerable<INotification> ExtractDomainEventsFromAggregates();
}
