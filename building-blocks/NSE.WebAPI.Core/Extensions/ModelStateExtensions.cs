using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NSE.WebAPI.Core.Extensions;

public static class ModelStateExtensions
{
    public static string[] GetErrors(this ModelStateDictionary modelState)
    {
        ArgumentNullException.ThrowIfNull(modelState);

        var errors = modelState.Keys
            .SelectMany(key => modelState[key]!.Errors)
            .Select(modelError => modelError.ErrorMessage)
            .Where(errorMessage => !string.IsNullOrWhiteSpace(errorMessage))
            .ToArray();

        return errors;
    }
}
