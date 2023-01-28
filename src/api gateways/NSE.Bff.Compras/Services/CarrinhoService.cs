using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Core.Communication;
using NSE.WebAPI.Core.HttpResponses;

namespace NSE.Bff.Compras.Services;

public interface ICarrinhoService
{
    Task<CarrinhoDTO> ObterCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher);
}

public class CarrinhoService : Service, ICarrinhoService
{
    private readonly HttpClient _httpClient;

    public CarrinhoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
    }

    public async Task<CarrinhoDTO> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/carrinho/");

        TratarErrosResponse(response);

        var responseDeserializado = await DeserializarObjetoResponse<CarrinhoDTO>(response);

        return responseDeserializado;
    }

    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto)
    {
        var itemContent = ParaConteudoHttp(produto);

        var response = await _httpClient.PostAsync("/carrinho/", itemContent);

        if (!TratarErrosResponse(response))
        {
            var responseDeserializado =  await DeserializarObjetoResponse<HttpOkResponse<ResponseResult>>(response);
            return responseDeserializado.Result;
        }

        return Ok();
    }

    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho)
    {
        var itemContent = ParaConteudoHttp(carrinho);

        var response = await _httpClient.PutAsync($"/carrinho/{carrinho.ProdutoId}", itemContent);

        if (!TratarErrosResponse(response))
        {
            var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<ResponseResult>>(response);
            return responseDeserializado.Result;
        }

        return Ok();
    }

    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");

        if (!TratarErrosResponse(response))
        {
            var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<ResponseResult>>(response);
            return responseDeserializado.Result;
        }

        return Ok();
    }

    public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher)
    {
        var itemContent = ParaConteudoHttp(voucher);

        var response = await _httpClient.PostAsync("/carrinho/aplicar-voucher/", itemContent);

        if (!TratarErrosResponse(response))
        {
            var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<ResponseResult>>(response);
            return responseDeserializado.Result;
        }

        return Ok();
    }
}