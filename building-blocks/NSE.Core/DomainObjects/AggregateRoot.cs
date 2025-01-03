﻿using NSE.Core.Messages;

namespace NSE.Core.DomainObjects;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();
   
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent @domainEvent) => _domainEvents.Add(@domainEvent);

    public void RemoveDomainEvent(DomainEvent @domainEvent) => _domainEvents.Remove(@domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}