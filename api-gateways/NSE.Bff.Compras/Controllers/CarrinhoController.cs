using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Bff.Compras.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICatalogoService _catalogoService;
    private readonly IPedidoService _pedidoService;

    public CarrinhoController(ICarrinhoService carrinhoService, 
                              ICatalogoService catalogoService, 
                              IPedidoService pedidoService)
    {
        _carrinhoService = carrinhoService ?? throw new ArgumentNullException(nameof(carrinhoService));
        _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
    }

    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index()
    {
        var carrinho = await _carrinhoService.ObterCarrinho();

        if (carrinho is null) return HttpNotFound();

        return HttpOk(carrinho);
    }

    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var quantidade = await _carrinhoService.ObterCarrinho();
        return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
    }

    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade, true);
        if (!OperacaoValida()) return HttpBadRequest();

        itemProduto.Nome = produto.Nome;
        itemProduto.Valor = produto.Valor;
        itemProduto.Imagem = produto.Imagem;

        var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);

        return HttpOk(resposta);
    }

    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade);
        if (!OperacaoValida()) return HttpBadRequest();

        var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

        return HttpOk(resposta);
    }

    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);

        if (produto == null)
        {
            AdicionarErroProcessamento("Produto inexistente!");
            return HttpNotFound();
        }

        var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);

        return HttpOk(resposta);
    }

    [HttpPost]
    [Route("compras/carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
    {
        var voucher = await _pedidoService.ObterVoucherPorCodigo(voucherCodigo);
        if (voucher is null)
        {
            AdicionarErroProcessamento("Voucher inválido ou não encontrado!");
            return HttpBadRequest();
        }

        var resposta = await _carrinhoService.AplicarVoucherCarrinho(voucher);

        return HttpOk(resposta);
    }

    private async Task ValidarItemCarrinho(ItemProdutoDTO produto, int quantidade, bool adicionarProduto = false)
    {
        if (produto is null) AdicionarErroProcessamento("Produto inexistente!");
        if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto.Nome}");

        var carrinho = await _carrinhoService.ObterCarrinho();
        var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto.Id);

        if (itemCarrinho is not null && adicionarProduto && itemCarrinho.Quantidade + quantidade > produto.QuantidadeEstoque)
        {
            AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
            return;
        }

        if (quantidade > produto.QuantidadeEstoque) AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }
}
