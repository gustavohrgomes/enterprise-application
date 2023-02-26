namespace NSE.Core.Messages.IntegrationEvents;

public sealed record UsuarioRegistradoIntegrationEvent(Guid Id, string Nome, string Email, string Cpf) : IntegrationEvent
{
}
