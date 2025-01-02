using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Events;

public sealed record ClienteRegistradoEvent(Guid Id, string Nome, string Email, string Cpf) : DomainEvent(Id)
{
}