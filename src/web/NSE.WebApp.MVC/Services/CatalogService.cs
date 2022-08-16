using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<ProdutoViewModel> ObterPorId(Guid id);
}

public class CatalogService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/catalogo/produtos/{id}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<ProdutoViewModel>(response);
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        var response = await _httpClient.GetAsync("/api/catalogo/produtos");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
    }
}
