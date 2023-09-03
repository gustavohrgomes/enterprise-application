using MassTransit;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Services;

public class PedidoCanceladoConsumer : IConsumer<PedidoCanceladoIntegrationEvent>
{
    private readonly ILogger<PedidoCanceladoConsumer> _logger;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PedidoCanceladoConsumer(
        ILogger<PedidoCanceladoConsumer> logger, 
        IPedidoRepository pedidoRepository, 
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<PedidoCanceladoIntegrationEvent> context)
    {
        _logger.LogInformation("Cancelando pedido {0} do cliente {1}", context.Message.PedidoId, context.Message.ClienteId);

        var pedido = await _pedidoRepository.ObterPorId(context.Message.PedidoId);
        
        pedido.CancelarPedido();

        _pedidoRepository.Atualizar(pedido);

        if (!await _unitOfWork.CommitAsync())
            throw new DomainException($"Problemas ao cancelar o pedido {context.Message.PedidoId}");

        _logger.LogInformation("Pedido {0} cancelado com sucesso", context.Message.PedidoId);
    }
}