using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Carrinho.API.Services;

public class PedidoRealizadoConsumer : IConsumer<PedidoRealizadoIntegrationEvent>
{
    private readonly CarrinhoContext _context;

    public PedidoRealizadoConsumer(CarrinhoContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<PedidoRealizadoIntegrationEvent> context)
    {
        var carrinho = await _context.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == context.Message.ClienteId);

        if (carrinho is not null)
        {
            _context.CarrinhoCliente.Remove(carrinho);
            await _context.SaveChangesAsync();
        }
    }
}