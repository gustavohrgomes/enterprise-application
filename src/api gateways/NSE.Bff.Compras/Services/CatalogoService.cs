using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;

namespace NSE.Bff.Compras.Services;

public interface ICatalogoService
{
    Task<ItemProdutoDTO> ObterPorId(Guid id);
    Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> itensCarrinhoIds);
}

public class CatalogoService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
    }
        
    public async Task<ItemProdutoDTO> ObterPorId(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/catalogo/produtos/{id}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<ItemProdutoDTO>(response);
    }

    public async Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> itensProdutoIds)
    {
        var idsRequest = string.Join(",", itensProdutoIds);

        var response = await _httpClient.GetAsync($"/api/catalogo/produtos/lista/{idsRequest}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<IEnumerable<ItemProdutoDTO>>(response);
    }
}