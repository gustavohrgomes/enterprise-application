namespace NSE.Core.Messages;

public abstract record Message
{
    protected Message()
    {
        MessageType = GetType().Name;
    }

    public string MessageType { get; init ; }
    public Guid AggregateId { get; init; }
}
