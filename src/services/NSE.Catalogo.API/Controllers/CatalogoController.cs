using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.HttpResponses;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.API.Controllers;

[Route("api/catalogo")]
[Authorize]
public class CatalogoController : MainController
{
    private readonly IProdutoRepository _produtoRepository;

    public CatalogoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
    }

    [AllowAnonymous]
    [HttpGet("produtos")]
    public async Task<ActionResult<PagedResult<Produto>>> Index([FromQuery] PaginationFilter pagedResultFilter)
    {
        var catalogoPaginado = await _produtoRepository.ObterTodosPaginados(pagedResultFilter);

        return HttpOk(catalogoPaginado);
    }

    [AllowAnonymous]
    [HttpGet("produtos/{id}")]
    public async Task<ActionResult<Produto>> ProdutoDetalhe(Guid id)
    {
        var produto = await _produtoRepository.ObterPorId(id);

        return HttpOk(produto);
    }

    [HttpGet("produtos/lista/{ids}")]
    public async Task<ActionResult<IEnumerable<Produto>>> ObterProdutosPorId(string ids)
    {
        var produtos = await _produtoRepository.ObterProdutosPorId(ids);

        return HttpOk(produtos);
    }
}
