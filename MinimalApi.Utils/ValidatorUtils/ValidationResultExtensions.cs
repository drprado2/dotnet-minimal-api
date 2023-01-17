using FluentValidation.Results;

namespace MinimalApi.Utils.ValidatorUtils;

public static class ValidationResultExtensions
{
    public static string ToMessageError(this ValidationResult validationResult)
    {
        // return string.Join(" ", validationResult.Errors.Select(failure =>
        //     $"""The field {failure.PropertyName} is invalid due to {failure.ErrorMessage}"""
        //     ));
        return string.Join(" ", validationResult.Errors.Select(failure =>failure.ErrorMessage));
    }
}
