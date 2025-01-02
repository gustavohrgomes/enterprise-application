using NSE.Core.Communication;
using NSE.WebAPI.Core.HttpResponses;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services;

public interface IComprasBffService
{
    // Carrinho
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<int> ObterQuantidadeCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(string voucher);

    // Pedido
    Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
    Task<PedidoViewModel> ObterUltimoPedido();
    Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
    PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco = null);
}

public class ComprasBffService : Service, IComprasBffService
{
    private readonly HttpClient _httpClient;

    public ComprasBffService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #region Carrinho

    public async Task<CarrinhoViewModel> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/compras/carrinho/");

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<CarrinhoViewModel>>(response);

        return responseDeserializado?.Result;
    }

    public async Task<int> ObterQuantidadeCarrinho()
    {
        var response = await _httpClient.GetAsync("/compras/carrinho-quantidade/");

        await TratarResponseAsync(response);

        return await DeserializarObjetoResponse<int>(response);
    }

    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho)
    {
        var itemContent = ParaConteudoHttp(carrinho);

        var response = await _httpClient.PostAsync("/compras/carrinho/items/", itemContent);

        if (!await TratarResponseAsync(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return Ok();
    }

    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel item)
    {
        var itemContent = ParaConteudoHttp(item);

        var response = await _httpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemContent);

        if (!await TratarResponseAsync(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return Ok();
    }

    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");

        if (!await TratarResponseAsync(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return Ok();
    }

    public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
    {
        var itemContent = ParaConteudoHttp(voucher);

        var response = await _httpClient.PostAsync("/compras/carrinho/aplicar-voucher/", itemContent);

        if (!await TratarResponseAsync(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return Ok();
    }

    #endregion Carrinho

    #region Pedido

    public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
    {
        var pedidoTransacaoContent = ParaConteudoHttp(pedidoTransacao);

        var response = await _httpClient.PostAsync("/compras/pedido", pedidoTransacaoContent);

        var result = await TratarResponseAsync(response);

        if (result) return Ok();
        
        var responseResult = await DeserializarObjetoResponse<ResponseResult>(response);
        return responseResult;
    }

    public async Task<PedidoViewModel> ObterUltimoPedido()
    {
        var response = await _httpClient.GetAsync("/compras/pedido/ultimo");

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<PedidoViewModel>>(response);

        return responseDeserializado.Result;
    }

    public async Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId()
    {
        var response = await _httpClient.GetAsync("/compras/pedido/lista-cliente/");

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<IEnumerable<PedidoViewModel>>>(response);

        return responseDeserializado.Result;
    }

    public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco = null)
    {
        var pedido = new PedidoTransacaoViewModel
        {
            ValorTotal = carrinho.ValorTotal,
            Itens = carrinho.Itens,
            Desconto = carrinho.Desconto,
            VoucherUtilizado = carrinho.VoucherUtilizado,
            VoucherCodigo = carrinho.Voucher?.Codigo
        };

        if (endereco != null)
        {
            pedido.Endereco = new EnderecoViewModel
            {
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Cep = endereco.Cep,
                Complemento = endereco.Complemento,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado
            };
        }

        return pedido;
    }

    #endregion Pedido
}
