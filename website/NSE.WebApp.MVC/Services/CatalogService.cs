using NSE.WebAPI.Core.HttpResponses;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<PagedViewModel<ProdutoViewModel>> ObterTodosPaginado(PaginationFilter pagination);
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

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<ProdutoViewModel>>(response);

        return responseDeserializado?.Result;
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        var response = await _httpClient.GetAsync("/api/catalogo/produtos");

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<IEnumerable<ProdutoViewModel>>>(response);

        return responseDeserializado?.Result;
    }

    public async Task<PagedViewModel<ProdutoViewModel>> ObterTodosPaginado(PaginationFilter pagination)
    {
        var response = await _httpClient.GetAsync($"/api/catalogo/produtos?PageSize={pagination.PageSize}&PageIndex={pagination.PageIndex}&Query={pagination.Query}");

        await TratarResponseAsync(response);

        var responseDeserializado = await DeserializarObjetoResponse<HttpOkResponse<PagedViewModel<ProdutoViewModel>>>(response);

        return responseDeserializado?.Result;
    }
}
