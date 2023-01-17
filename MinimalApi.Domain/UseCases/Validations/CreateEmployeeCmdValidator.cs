using FluentValidation;
using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.UseCases.Commands;

namespace MinimalApi.Domain.UseCases.Validations;

public class CreateEmployeeCmdValidator : AbstractValidator<CreateEmployeeCmd>, IValidator<CreateEmployeeCmd>
{
    private readonly ICompanyRepository _companyRepository;

    public CreateEmployeeCmdValidator(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
        RuleFor(e => e.AuthenticatedUserId).NotEmpty();
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.Email).EmailAddress().NotEmpty();
        RuleFor(e => e.Phone).Matches("^([0-9])+$");
        RuleFor(e => e.IdentityUserId).NotEmpty();
        RuleFor(e => e.CompanyId).MustAsync(async (companyId, token) =>
        {
            var c = await _companyRepository.GetByIdAsync(companyId);
            return c?.Active ?? false;
        });
    }
}
