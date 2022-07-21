using MediatR;

namespace NSE.Clientes.API.Application.Events;

public class ClienteEventHandler : INotificationHandler<ClienteRegistradoEvent>
{
    public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
    {
        // Faz alguma coisa
        return Task.CompletedTask;
    }
}
