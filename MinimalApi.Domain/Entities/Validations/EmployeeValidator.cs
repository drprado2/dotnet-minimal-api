using FluentValidation;

namespace MinimalApi.Domain.Entities.Validations;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        RuleFor(e => e.IdentityUserId).NotEmpty();
        RuleFor(e => e.CompanyId).NotEmpty();
    }
}