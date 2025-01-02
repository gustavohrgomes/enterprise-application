using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Models;
using NSE.Core.Data;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands;

public class ClienteCommandHandler : CommandHandler,
    IRequestHandler<RegistrarClienteCommand, ValidationResult>,
    IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteCommandHandler(IClienteRepository clienteRepository, 
                                 IPublisher publisher, 
                                 IUnitOfWork unitOfWork)
        : base(publisher)
    {
        _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido()) return message.ValidationResult;

        var cliente = Cliente.Create(message.Id, message.Nome, message.Email, message.Cpf);

        var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

        if (clienteExistente is not null)
        {
            AdicionarErro("CPF já cadastrado.");
            return ValidationResult;
        }

        _clienteRepository.Adicionar(cliente);

        cliente.AddDomainEvent(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

        return await PersistirDados(_unitOfWork);
    }

    public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido()) return message.ValidationResult;

        var endereco = new Endereco(message.Logradouro, message.Numero, message.Bairro, message.Cep, message.Cidade, message.Estado, message.ClienteId, message.Complemento);
        
        _clienteRepository.AdicionarEndereco(endereco);

        return await PersistirDados(_unitOfWork);
    }
}
