using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Core.Messages.Integration;

public sealed record PedidoAutorizadoIntegrationEvent(Guid ClienteId, Guid PedidoId, IDictionary<Guid, int> Itens) : IntegrationEvent
{
}