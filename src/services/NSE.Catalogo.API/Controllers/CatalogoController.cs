﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using NSE.WebAPI.Core.Controllers;
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
    public async Task<PagedResult<Produto>> Index([FromQuery] PaginationFilter pagedResultFilter) 
        => await _produtoRepository.ObterTodosPaginados(pagedResultFilter);

    [AllowAnonymous]
    [HttpGet("produtos/{id}")]
    public async Task<Produto> ProdutoDetalhe(Guid id) => await _produtoRepository.ObterPorId(id);

    [HttpGet("produtos/lista/{ids}")]
    public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids)
        => await _produtoRepository.ObterProdutosPorId(ids);
}
