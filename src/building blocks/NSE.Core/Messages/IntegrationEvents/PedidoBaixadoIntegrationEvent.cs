namespace NSE.Core.Messages.IntegrationEvents;

public sealed record PedidoBaixadoIntegrationEvent(Guid ClienteId, Guid PedidoId) : IntegrationEvent
{
}
