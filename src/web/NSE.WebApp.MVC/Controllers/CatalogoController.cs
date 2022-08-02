﻿using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers;

public class CatalogoController : MainController
{
    private readonly ICatalogoService _catalogoService;

    public CatalogoController(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
    }

    [Route("")]
    [HttpGet("vitrine")]
    public async Task<IActionResult> Index()
    {
        var produtos = await _catalogoService.ObterTodos();

        return View(produtos);
    }

    [HttpGet("produto-detalhe/{id}")]
    public async Task<IActionResult> ProdutoDetalhe(Guid id)
    {
        var produtos = await _catalogoService.ObterPorId(id);

        return View(produtos);
    }
}
