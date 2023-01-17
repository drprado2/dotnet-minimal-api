using FluentValidation;
using MinimalApi.Domain.UseCases.Commands;

namespace MinimalApi.Domain.UseCases.Validations;

public class CreateCompanyCmdValidator : AbstractValidator<CreateCompanyCmd>, IValidator<CreateCompanyCmd>
{
    public CreateCompanyCmdValidator()
    {
        RuleFor(x => x.Document).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}