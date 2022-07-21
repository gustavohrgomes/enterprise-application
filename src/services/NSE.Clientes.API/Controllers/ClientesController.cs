using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Communication;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Clientes.API.Controllers;

[Route("api/[controller]")]
public class ClientesController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;

    public ClientesController(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    [HttpGet("clientes")]
    public async Task<IActionResult> Index()
    {
        var resultado = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(), "Eduardo", "edu@edu.com", "30314299076"));

        return CustomResponse(resultado);
    }
}
