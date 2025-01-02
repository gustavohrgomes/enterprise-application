using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Core.Communication;
using NSE.WebAPI.Core.HttpResponses;
using System.Net;

namespace NSE.Bff.Compras.Services;

public interface IPedidoService
{
    Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);
    Task<PedidoDTO> ObterUltimoPedido();
    Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();
    Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
}

public class PedidoService : Service, IPedidoService
{
    private readonly HttpClient _httpClient;

    public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
    }

    public async Task<ResponseResult> FinalizarPedido(PedidoDTO pedido)
    {
        var pedidoContent = ParaConteudoHttp(pedido);

        var response = await _httpClient.PostAsync("/pedido/", pedidoContent);

        if (TratarErrosResponse(response)) return ResponseResult.Ok();

        var responseResult = await DeserializarObjetoResponse<ResponseResult>(response);
        
        return ResponseResult.BadRequest(responseResult.Errors);
    }

    public async Task<PedidoDTO> ObterUltimoPedido()
    {
        var response = await _httpClient.GetAsync("/pedido/ultimo/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<PedidoDTO>>(response);

        return responseDeserializado.Result;
    }

    public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
    {
        var response = await _httpClient.GetAsync("/pedido/lista-cliente/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<IEnumerable<PedidoDTO>>>(response);

        return responseDeserializado.Result;
    }

    public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
    {
        var response = await _httpClient.GetAsync($"/voucher/{codigo}/");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        TratarErrosResponse(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<VoucherDTO>>(response);

        return responseDeserializado.Result;
    }
}