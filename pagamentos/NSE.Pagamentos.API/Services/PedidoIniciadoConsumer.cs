using MassTransit;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services;

public class PedidoIniciadoConsumer : IConsumer<PedidoIniciadoIntegrationEvent>
{
    private readonly IPagamentoService _pagamentoService;

    public PedidoIniciadoConsumer(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    public async Task Consume(ConsumeContext<PedidoIniciadoIntegrationEvent> context)
    {
        var pagamento = new Pagamento
        {
            PedidoId = context.Message.PedidoId,
            TipoPagamento = (TipoPagamento)context.Message.TipoPagamento,
            Valor = context.Message.Valor,
            CartaoCredito = new CartaoCredito(
                context.Message.NomeCartao, 
                context.Message.NumeroCartao, 
                context.Message.MesAnoVencimento,
                context.Message.CVV)
        };

        var result = await _pagamentoService.AutorizarPagamento(pagamento);

        await context.RespondAsync(result);
    }
}