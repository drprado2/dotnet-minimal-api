using MinimalApi.Domain.Errors;
using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Domain.UseCases.Validations;
using MinimalApi.Utils.ValidatorUtils;

namespace MinimalApi.Domain.UseCases;

public class GetCompanyEmployeesHandler : IUseCaseHandler<GetCompanyEmployeesQuery>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IValidator<GetCompanyEmployeesQuery> _validator;

    public GetCompanyEmployeesHandler(IEmployeeRepository employeeRepository, IValidator<GetCompanyEmployeesQuery> validator)
    {
        _employeeRepository = employeeRepository;
        _validator = validator;
    }
    
    public async Task<GetCompanyEmployeesQuery> ExecuteAsync(GetCompanyEmployeesQuery input)
    {
        var result = await _validator.ValidateAsync(input);
        if (!result.IsValid)
        {
            throw new InvalidInputException(result.ToMessageError());
        }
        
        var employees = await _employeeRepository.GetEmployeesFromCompanyAsync(input.CompanyId, input.PageSize, input.LastPageItem);
        var employeesList = employees.ToList();
        var lastPageId = employeesList.LastOrDefault()?.InternalId ;
        return input with { EmployeesRetrieved = employeesList, LastPageItem = lastPageId};
    }
}