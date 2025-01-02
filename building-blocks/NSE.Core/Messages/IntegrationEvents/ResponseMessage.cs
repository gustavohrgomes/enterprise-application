using FluentValidation.Results;

namespace NSE.Core.Messages.IntegrationEvents;

public record ResponseMessage : Message
{
    public ResponseMessage(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    public ValidationResult ValidationResult { get; set; }
}
