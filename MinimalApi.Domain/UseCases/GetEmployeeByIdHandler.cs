using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.UseCases.Queries;

namespace MinimalApi.Domain.UseCases;

public class GetEmployeeByIdHandler : IUseCaseHandler<GetEmployeeByIdQuery>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<GetEmployeeByIdQuery> ExecuteAsync(GetEmployeeByIdQuery input)
    {
        var employee = await _employeeRepository.GetByIdAsync(input.EmployeeId);
        return input with { EmployeeRetrieved = employee};
    }
}
