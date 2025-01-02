namespace NSE.Core.Messages.IntegrationEvents;

public sealed record PedidoCanceladoIntegrationEvent(Guid ClienteId, Guid PedidoId) : IntegrationEvent
{
}
