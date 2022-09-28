using NSE.Catalogo.API.Models;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.Core.Messages.IntegrationEvents;
using NSE.MessageBus;

namespace NSE.Catalogo.API.Services;

public class CatalogoIntegrationHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageBus _bus;
    private readonly ILogger<CatalogoIntegrationHandler> _logger;

    public CatalogoIntegrationHandler(IServiceProvider serviceProvider, 
                                      IMessageBus bus, 
                                      ILogger<CatalogoIntegrationHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Catalogo Integration Handler está em execução");
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers()
    {
        _bus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request => await BaixarEstoque(request));
    }

    private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        _logger.LogInformation("Dando baixa no estoque para o pedido {0}", message.PedidoId);
        using (var scope = _serviceProvider.CreateScope())
        {
            var produtosComEstoque = new List<Produto>();
            var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();

            var idsProdutos = string.Join(',', message.Itens.Select(c => c.Key));
            var produtos = await produtoRepository.ObterProdutosPorId(idsProdutos);

            if (produtos.Count != message.Itens.Count)
            {
                _logger.LogInformation("Quantidade do produto é diferente da quantidade de itens no pedido {0}", message.PedidoId);
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtos)
            {
                var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;

                if (produto.EstaDisponivel(quantidadeProduto))
                {
                    produto.RetirarEstoque(quantidadeProduto);
                    produtosComEstoque.Add(produto);
                }
            }

            if (produtosComEstoque.Count != message.Itens.Count)
            {
                _logger.LogInformation("Quantidade do produto com estoque é diferente da quantidade de itens no pedido {0}", message.PedidoId);
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtosComEstoque)
            {
                produtoRepository.Atualizar(produto);
            }

            if (!await produtoRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException($"Problemas ao atualizar estoque do pedido {message.PedidoId}");
            }

            var pedidoBaixado = new PedidoBaixadoIntegrationEvent(message.ClienteId, message.PedidoId);
            await _bus.PublishAsync(pedidoBaixado);
            _logger.LogInformation("Produtos do pedido {0} foram baixados com sucesso.", message.PedidoId);
        }
    }

    private async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.ClienteId, message.PedidoId);
        await _bus.PublishAsync(pedidoCancelado);
    }
}
