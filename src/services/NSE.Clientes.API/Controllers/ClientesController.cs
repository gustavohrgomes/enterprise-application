﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Models;
using NSE.Core.Communication;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.API.Controllers;

[Authorize]
public class ClientesController : MainController
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMediatorHandler _mediator;
    private readonly IAspNetUser _user;

    public ClientesController(IClienteRepository clienteRepository, IMediatorHandler mediator, IAspNetUser user)
    {
        _clienteRepository = clienteRepository;
        _mediator = mediator;
        _user = user;
    }

    [HttpGet("cliente/endereco")]
    public async Task<IActionResult> ObterEndereco()
    {
        var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());

        return endereco == null ? HttpNotFound() : HttpOk(endereco);
    }

    [HttpPost("cliente/endereco")]
    public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
    {
        endereco.ClienteId = _user.ObterUserId();
        return HttpOk(await _mediator.EnviarComando(endereco));
    }
}
