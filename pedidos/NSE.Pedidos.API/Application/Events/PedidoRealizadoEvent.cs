using NSE.Core.Messages;

namespace NSE.Pedidos.API.Application.Events;

public sealed record PedidoRealizadoEvent(Guid PedidoId, Guid ClienteId) : DomainEvent(PedidoId)
{
}