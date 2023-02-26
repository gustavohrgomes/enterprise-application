using MediatR;

namespace NSE.Core.Messages;

public abstract record DomainEvent(Guid AggragateId) : Message, INotification
{
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
