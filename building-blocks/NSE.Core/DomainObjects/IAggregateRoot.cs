using NSE.Core.Messages;

namespace NSE.Core.DomainObjects;

public interface IAggregateRoot
{
    public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    public void AddDomainEvent(DomainEvent @domainEvent);
    public void RemoveDomainEvent(DomainEvent @domainEvent);
    public void ClearDomainEvents();
}
