using FluentValidation.Results;
using NSE.Core.Messages;

namespace NSE.Core.Communication;

public interface IMediatorHandler
{
    Task PublicarEvento<T>(T evento) where T : DomainEvent;
    Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
}
