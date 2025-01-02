using MediatR;

namespace NSE.Core.Messages;

public abstract record Event : Message, INotification
{
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
