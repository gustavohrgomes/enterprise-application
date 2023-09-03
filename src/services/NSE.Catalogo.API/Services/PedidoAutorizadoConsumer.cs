using MassTransit;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.Core.Messages.IntegrationEvents;

namespace NSE.Catalogo.API.Services;

public class PedidoAutorizadoConsumer : IConsumer<PedidoAutorizadoIntegrationEvent>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly Logger<PedidoAutorizadoConsumer> _logger;
    private readonly IPublishEndpoint _bus;
    private readonly IUnitOfWork _unitOfWork;

    public PedidoAutorizadoConsumer(
        IProdutoRepository produtoRepository,
        Logger<PedidoAutorizadoConsumer> logger, 
        IPublishEndpoint bus, 
        IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
        _bus = bus;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<PedidoAutorizadoIntegrationEvent> context)
    {
        _logger.LogInformation("Dando baixa no estoque para o pedido {0}", context.Message.PedidoId);
        
        var produtosComEstoque = new List<Produto>();
        
        var idsProdutos = string.Join(',', context.Message.Itens.Select(c => c.Key));
        var produtos = await _produtoRepository.ObterProdutosPorId(idsProdutos);
        
        if (produtos.Count != context.Message.Itens.Count)
        {
            _logger.LogInformation("Quantidade do produto é diferente da quantidade de itens no pedido {0}", context.Message.PedidoId);
            await _bus.Publish<PedidoCanceladoIntegrationEvent>(
                new (context.Message.ClienteId, context.Message.PedidoId));
            return;
        }
        
        foreach (var produto in produtos)
        {
            var quantidadeProduto = context.Message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;

            if (!produto.EstaDisponivel(quantidadeProduto)) continue;

            produto.RetirarEstoque(quantidadeProduto);
            produtosComEstoque.Add(produto);
        }
        
        if (produtosComEstoque.Count != context.Message.Itens.Count)
        {
            _logger.LogInformation("Quantidade do produto é diferente da quantidade de itens no pedido {0}", context.Message.PedidoId);
            await _bus.Publish<PedidoCanceladoIntegrationEvent>(
                new (context.Message.ClienteId, context.Message.PedidoId));
            return;
        }
        
        foreach (var produto in produtosComEstoque)
        {
            _produtoRepository.Atualizar(produto);
        }
        
        if (!await _unitOfWork.CommitAsync())
        {
            throw new DomainException($"Problemas ao atualizar estoque do pedido {context.Message.PedidoId}");
        }

        _bus.Publish<PedidoBaixadoIntegrationEvent>(new (
            context.Message.ClienteId,
            context.Message.PedidoId));
        
        _logger.LogInformation("Produtos do pedido {0} foram baixados com sucesso.", context.Message.PedidoId);
    }
}