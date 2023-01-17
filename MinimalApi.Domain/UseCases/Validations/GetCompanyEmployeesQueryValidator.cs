using FluentValidation;
using MinimalApi.Domain.UseCases.Queries;

namespace MinimalApi.Domain.UseCases.Validations;

public class GetCompanyEmployeesQueryValidator : AbstractValidator<GetCompanyEmployeesQuery>, IValidator<GetCompanyEmployeesQuery>
{
    public GetCompanyEmployeesQueryValidator()
    {
        RuleFor(f => f.CompanyId).NotEmpty();
        RuleFor(f => f.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(200);
        RuleFor(f => f.LastPageItem).GreaterThanOrEqualTo(0);
    }
}