using FluentValidation.Results;

namespace MinimalApi.Domain.UseCases.Validations;

public interface IValidator<T> where T: notnull
{
    public Task<ValidationResult> ValidateAsync(T command, CancellationToken cancellation = new());
}