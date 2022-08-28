using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Core.Messages.IntegrationEvents;
using NSE.MessageBus;

namespace NSE.Carrinho.API.Services;

public class CarrinhoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public CarrinhoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscriber();
        return Task.CompletedTask;
    }

    private void SetSubscriber()
    {
        _bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request => await ApagarCarrinho(request));
    }

    private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

        var carrinho = await context.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

        if (carrinho is not null)
        {
            context.CarrinhoCliente.Remove(carrinho);
            await context.SaveChangesAsync();
        }
    }
}
