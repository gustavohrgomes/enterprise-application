using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Bff.Compras.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index() => CustomResponse();

    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<IActionResult> ObterQuantidadeCarrinho() => CustomResponse();

    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho() => CustomResponse();

    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho() => CustomResponse();

    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho() => CustomResponse();
}
