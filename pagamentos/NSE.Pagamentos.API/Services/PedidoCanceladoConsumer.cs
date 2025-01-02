using MassTransit;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Pagamentos.API.Services;

public class PedidoCanceladoConsumer : IConsumer<PedidoCanceladoIntegrationEvent>
{
    private readonly IPagamentoService _pagamentoService;

    public PedidoCanceladoConsumer(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    public async Task Consume(ConsumeContext<PedidoCanceladoIntegrationEvent> context)
    {
        var response = await _pagamentoService.CancelarPagamento(context.Message.PedidoId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Falha ao cancelar pagamento do pedido {context.Message.PedidoId}");
    }
}