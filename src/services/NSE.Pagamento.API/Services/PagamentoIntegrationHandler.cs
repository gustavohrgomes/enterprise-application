using NSE.Core.Messages.IntegrationEvents;
using NSE.MessageBus;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services;

public class PagamentoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public PagamentoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    private void SetResponder()
    {
        _bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request => await AutorizarPagamento(request));
    }

    private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
    {
        ResponseMessage response;

        using (var scope = _serviceProvider.CreateScope())
        {
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var pagamento = new Pagamento
            {
                PedidoId = message.PedidoId,
                TipoPagamento = (TipoPagamento)message.TipoPagamento,
                Valor = message.Valor,
                CartaoCredito = new CartaoCredito(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
            };

            response = await pagamentoService.AutorizarPagamento(pagamento);

            return response;
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }
}