using NSE.Core.DomainObjects;
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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetResponder() 
        => _bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request => await AutorizarPagamento(request));

    private void SetSubscribers()
    {
        _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request => await CancelarPagamento(request));

        _bus.SubscribeAsync<PedidoBaixadoIntegrationEvent>("PedidoBaixadoEstoque", async request => await CapturarPagamento(request));
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

    private async Task CancelarPagamento(PedidoCanceladoIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var response = await pagamentoService.CancelarPagamento(message.PedidoId);

            if (!response.ValidationResult.IsValid)
                throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
        }
    }

    private async Task CapturarPagamento(PedidoBaixadoIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var response = await pagamentoService.CapturarPagamento(message.PedidoId);

            if (!response.ValidationResult.IsValid)
                throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");

            await _bus.PublishAsync(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));
        }
    }
}