using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Models;
using NSE.Core.Messages;
using NSE.Core.Utils;

namespace NSE.Clientes.API.Application.Commands;

public class ClienteCommandHandler : CommandHandler,
    IRequestHandler<RegistrarClienteCommand, ValidationResult>,
    IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
    }

    public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido()) return message.ValidationResult;

        var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf.ApenasNumeros());

        var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

        // TODO: caso cliente já esteja cadastrado, adicionar erro
        if (clienteExistente is not null)
        {
            AdicionarErro("Este CPF já está cadastrado");
            return ValidationResult;
        }

        _clienteRepository.Adicionar(cliente);

        cliente.AddDomainEvent(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

        return await PersistirDados(_clienteRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido()) return message.ValidationResult;

        var endereco = new Endereco(message.Logradouro, message.Numero, message.Bairro, message.Cep, message.Cidade, message.Estado, message.ClienteId, message.Complemento);
        
        _clienteRepository.AdicionarEndereco(endereco);

        return await PersistirDados(_clienteRepository.UnitOfWork);
    }
}
