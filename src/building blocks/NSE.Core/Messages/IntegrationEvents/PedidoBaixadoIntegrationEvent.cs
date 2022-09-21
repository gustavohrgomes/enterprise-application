namespace NSE.Core.Messages.IntegrationEvents;

public class PedidoBaixadoIntegrationEvent : IntegrationEvent
{
    public PedidoBaixadoIntegrationEvent(Guid clienteId, Guid pedidoId)
    {
        ClienteId = clienteId;
        PedidoId = pedidoId;
    }

    public Guid ClienteId { get; set; }
    public Guid PedidoId { get; set; }
}
