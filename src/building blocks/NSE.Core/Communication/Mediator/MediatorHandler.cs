using FluentValidation.Results;
using MediatR;
using NSE.Core.Messages;

namespace NSE.Core.Communication;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command
        => await _mediator.Send(comando);

    public async Task PublicarEvento<T>(T evento) where T : DomainEvent 
        => await _mediator.Publish(evento);
}
