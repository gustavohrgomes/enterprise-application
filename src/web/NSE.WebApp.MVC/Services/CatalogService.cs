using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services;

public class CatalogService : Service, ICatalogoService
{
    private readonly HttpClient _client;

    public CatalogService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        var response = await _client.GetAsync($"/api/catalogo/produtos/{id}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<ProdutoViewModel>(response);
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        var response = await _client.GetAsync("/api/catalogo/produtos");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
    }
}
