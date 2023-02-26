namespace NSE.Core.Messages.IntegrationEvents;

public sealed record PedidoPagoIntegrationEvent(Guid ClienteId, Guid PedidoId) : IntegrationEvent
{
}
