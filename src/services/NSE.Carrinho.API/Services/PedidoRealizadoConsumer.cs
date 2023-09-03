using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Carrinho.API.Services;

public class PedidoRealizadoConsumer : IConsumer<PedidoRealizadoIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public PedidoRealizadoConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<PedidoRealizadoIntegrationEvent> context)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

        var carrinho = await dbContext.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == context.Message.ClienteId);

        if (carrinho is not null)
        {
            dbContext.CarrinhoCliente.Remove(carrinho);
            await dbContext.SaveChangesAsync();
        }
    }
}