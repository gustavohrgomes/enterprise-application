﻿using FluentValidation.Results;
using MediatR;

namespace NSE.Core.Messages;

public abstract record Command : Message, IRequest<ValidationResult>
{
    protected Command()
    {
        Timestamp = DateTime.Now;
    }

    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    public abstract bool EhValido();
}
