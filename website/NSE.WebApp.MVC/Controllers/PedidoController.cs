using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    public class PedidoController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IComprasBffService _comprasBffService;

        public PedidoController(IClienteService clienteService,
            IComprasBffService comprasBffService)
        {
            _clienteService = clienteService;
            _comprasBffService = comprasBffService;
        }

        [HttpGet]
        [Route("endereco-de-entrega")]
        public async Task<IActionResult> EnderecoEntrega()
        {
            var carrinho = await _comprasBffService.ObterCarrinho();
            if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var endereco = await _clienteService.ObterEndereco();
            var pedido = _comprasBffService.MapearParaPedido(carrinho, endereco);

            return View(pedido);
        }

        [HttpGet]
        [Route("pagamento")]
        public async Task<IActionResult> Pagamento()
        {
            var carrinho = await _comprasBffService.ObterCarrinho();
            if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var pedido = _comprasBffService.MapearParaPedido(carrinho);

            return View(pedido);
        }

        [HttpPost]
        [Route("finalizar-pedido")]
        public async Task<IActionResult> FInalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            if (!ModelState.IsValid)
                return View("Pagamento", _comprasBffService.MapearParaPedido(await _comprasBffService.ObterCarrinho()));

            var retorno = await _comprasBffService.FinalizarPedido(pedidoTransacao);

            if (!ResponsePossuiErros(retorno)) return RedirectToAction("PedidoConcluido");

            var carrinho = await _comprasBffService.ObterCarrinho();

            if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var pedidoMapeado = _comprasBffService.MapearParaPedido(carrinho);

            return View("Pagamento", pedidoMapeado);
        }

        [HttpGet]
        [Route("pedido-concluido")]
        public async Task<IActionResult> PedidoConcluido()
            => View("ConfirmacaoPedido", await _comprasBffService.ObterUltimoPedido());

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MeusPedidos()
            => View(await _comprasBffService.ObterListaPorClienteId());
    }
}