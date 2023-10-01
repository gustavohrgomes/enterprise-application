using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;

namespace NSE.Core.Messages;

public abstract class CommandHandler
{
    protected readonly ValidationResult ValidationResult = new();

    protected void AddProcessingError(string errorMessage) 
        => ValidationResult.Errors.Add(new ValidationFailure(string.Empty, errorMessage));
}
