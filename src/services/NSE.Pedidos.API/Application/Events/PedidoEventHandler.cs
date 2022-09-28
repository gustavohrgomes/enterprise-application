using MediatR;
using NSE.Core.Messages.IntegrationEvents;
using NSE.MessageBus;

namespace NSE.Pedidos.API.Application.Events;

public class PedidoEventHandler : INotificationHandler<PedidoRealizadoEvent>
{
    private readonly IMessageBus _bus;

    public PedidoEventHandler(IMessageBus bus)
    {
        _bus = bus;
    }

    public Task Handle(PedidoRealizadoEvent message, CancellationToken cancellationToken)
        => _bus.PublishAsync(new PedidoRealizadoIntegrationEvent(message.ClienteId));
}
