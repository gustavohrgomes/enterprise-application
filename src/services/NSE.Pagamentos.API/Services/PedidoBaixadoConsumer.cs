using MassTransit;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Pagamentos.API.Services;

public class PedidoBaixadoConsumer : IConsumer<PedidoBaixadoIntegrationEvent>
{
    private readonly IPagamentoService _pagamentoService;
    private readonly IPublishEndpoint _bus;
    
    public PedidoBaixadoConsumer(IPagamentoService pagamentoService, IPublishEndpoint bus)
    {
        _pagamentoService = pagamentoService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<PedidoBaixadoIntegrationEvent> context)
    {
        var response = await _pagamentoService.CapturarPagamento(context.Message.PedidoId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Falha ao capturar pagamento do pedido {context.Message.PedidoId}");

        await _bus.Publish(new PedidoPagoIntegrationEvent(
            context.Message.ClienteId, 
            context.Message.PedidoId));
    }
}