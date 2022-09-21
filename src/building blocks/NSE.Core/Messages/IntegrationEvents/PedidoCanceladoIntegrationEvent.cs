namespace NSE.Core.Messages.IntegrationEvents;

public class PedidoCanceladoIntegrationEvent : IntegrationEvent
{
    public PedidoCanceladoIntegrationEvent(Guid clienteId, Guid pedidoId)
    {
        ClienteId = clienteId;
        PedidoId = pedidoId;
    }

    public Guid ClienteId { get; set; }
    public Guid PedidoId { get; set; }
}
