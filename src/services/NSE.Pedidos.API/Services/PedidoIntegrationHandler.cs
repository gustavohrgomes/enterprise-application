using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.IntegrationEvents;
using NSE.MessageBus;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Services;

public class PedidoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PedidoIntegrationHandler> _logger;

    public PedidoIntegrationHandler(IMessageBus messageBus, 
                                    IServiceProvider serviceProvider, 
                                    ILogger<PedidoIntegrationHandler> logger)
    {
        _bus = messageBus;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Pedido integration Handler está em execução.");
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers()
    {
        _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request => await CancelarPedido(request));

        _bus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago", async request => await FinalizarPedido(request));
    }

    private async Task CancelarPedido(PedidoCanceladoIntegrationEvent message)
    {
        _logger.LogInformation("Cancelando pedido {0} do cliente {1}", message.PedidoId, message.ClienteId);
        using var scope = _serviceProvider.CreateScope();

        var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepository.ObterPorId(message.PedidoId);
        pedido.CancelarPedido();

        pedidoRepository.Atualizar(pedido);

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        if (!await unitOfWork.CommitAsync())
            throw new DomainException($"Problemas ao cancelar o pedido {message.PedidoId}");

        _logger.LogInformation("Pedido {0} cancelado com sucesso", message.PedidoId);
    }

    private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
    {
        _logger.LogInformation("Finalizando pedido {0} do cliente {1}", message.PedidoId, message.ClienteId);
        using var scope = _serviceProvider.CreateScope();

        var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepository.ObterPorId(message.PedidoId);
        pedido.FinalizarPedido();

        pedidoRepository.Atualizar(pedido);

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        if (!await unitOfWork.CommitAsync())
            throw new DomainException($"Problemas ao finalizar o pedido {message.PedidoId}");

        _logger.LogInformation("Pedido {0} finalizado com sucesso", message.PedidoId);
    }
}
