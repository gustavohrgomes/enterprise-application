using MassTransit;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Communication;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Clientes.API.Services;

public sealed class RegistroClienteConsumer : IConsumer<UsuarioRegistradoIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public RegistroClienteConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<UsuarioRegistradoIntegrationEvent> context)
    {
        var clienteCommand = new RegistrarClienteCommand(context.Message.Id, context.Message.Nome,
            context.Message.Email, context.Message.Cpf);

        using var scope = _serviceProvider.CreateScope();
        
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        var result = await mediator.EnviarComando(clienteCommand);

        await context.RespondAsync(new ResponseMessage(result));
    }
}