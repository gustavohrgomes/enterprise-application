using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;

namespace NSE.Core.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult;
    private readonly IPublisher _publisher;

    protected CommandHandler(IPublisher publisher)
    {
        _publisher = publisher;
        ValidationResult = new ValidationResult();
    }

    protected void AdicionarErro(string mensagem) 
        => ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));

    protected async Task<ValidationResult> PersistirDados(IUnitOfWork unitOfWork)
    {
        if (!await unitOfWork.CommitAsync()) AdicionarErro("Houve um erro ao persistir os dados");

        var domainEvents = unitOfWork.ExtractDomainEventsFromAggregates();

        var domainEventsToDispatch = domainEvents.Select(async (domainEvent) => await _publisher.Publish(domainEvent));

        await Task.WhenAll(domainEventsToDispatch);

        return ValidationResult;
    }
}
