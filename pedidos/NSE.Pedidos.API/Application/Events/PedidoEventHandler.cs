using MassTransit;
using MediatR;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Pedidos.API.Application.Events;

public class PedidoEventHandler : INotificationHandler<PedidoRealizadoEvent>
{
    private readonly IPublishEndpoint _bus;

    public PedidoEventHandler(IPublishEndpoint bus)
    {
        _bus = bus;
    }

    public Task Handle(PedidoRealizadoEvent message, CancellationToken cancellationToken)
        => _bus.Publish(new PedidoRealizadoIntegrationEvent(message.ClienteId), cancellationToken);
}
