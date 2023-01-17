using FluentValidation;

namespace MinimalApi.Domain.Entities.Validations;

public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(c => c.Document).NotNull().Must(d => d.Validate());
        RuleFor(c => c.Name).NotEmpty();
    }
}