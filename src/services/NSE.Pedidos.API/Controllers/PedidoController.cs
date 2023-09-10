using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Communication;
using NSE.Pedidos.API.Application.Commands;
using NSE.Pedidos.API.Application.Queries;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pedidos.API.Controllers;

[Authorize]
public class PedidoController : MainController
{
    private readonly IMediatorHandler _mediator;
    private readonly IAspNetUser _user;
    private readonly IPedidoQueries _pedidoQueries;

    public PedidoController(IMediatorHandler mediator,
        IAspNetUser user,
        IPedidoQueries pedidoQueries)
    {
        _mediator = mediator;
        _user = user;
        _pedidoQueries = pedidoQueries;
    }

    [HttpPost("pedido")]
    public async Task<IActionResult> AdicionarPedido(AdicionarPedidoCommand pedido)
    {
        pedido.ClienteId = _user.ObterUserId();

        var result = await _mediator.EnviarComando(pedido);

        if (result.IsValid) return HttpOk();

        return HttpBadRequest(result);
    }

    [HttpGet("pedido/ultimo")]
    public async Task<IActionResult> UltimoPedido()
    {
        var pedido = await _pedidoQueries.ObterUltimoPedido(_user.ObterUserId());

        return pedido is null ? HttpNotFound() : HttpOk(pedido);
    }

    [HttpGet("pedido/lista-cliente")]
    public async Task<IActionResult> ListaPorCliente()
    {
        var pedidos = await _pedidoQueries.ObterListaPorClienteId(_user.ObterUserId());

        return pedidos is null ? HttpNotFound() : HttpOk(pedidos);
    }

    [AllowAnonymous]
    [HttpGet("pedido/autorizados")]
    public async Task<IActionResult> PedidosAutorizados()
    {
        var pedidos = await _pedidoQueries.ObterPedidosAutorizados();

        return pedidos is null ? HttpNotFound() : HttpOk(pedidos);
    }
}