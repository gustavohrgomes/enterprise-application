using NSE.Core.Communication;
using NSE.WebAPI.Core.HttpResponses;
using NSE.WebApp.MVC.Models;
using System.Net;

namespace NSE.WebApp.MVC.Services;

public interface IClienteService
{
    Task<EnderecoViewModel> ObterEndereco();
    Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
}

public class ClienteService : Service, IClienteService
{
    private readonly HttpClient _httpClient;

    public ClienteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EnderecoViewModel> ObterEndereco()
    {
        var response = await _httpClient.GetAsync("/cliente/endereco");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<EnderecoViewModel>>(response);

        return responseDeserializado.Result;
    }

    public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
    {
        var enderecoContent = ParaConteudoHttp(endereco);

        var response = await _httpClient.PostAsync("/cliente/endereco", enderecoContent);

        if (!await TratarResponseAsync(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return Ok();
    }
}
