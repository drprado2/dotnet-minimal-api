using MinimalApi.Domain.Errors;
using MinimalApi.Domain.Repositories;

namespace MinimalApi.Domain.Events.Consumers;

public class EmployeeCreatedConsumer : IEventConsumer<EmployeeCreated>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeCreatedConsumer(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
    }
    
    public async Task ConsumeAsync(EmployeeCreated @event)
    {
        var authenticatedEmployee = await  _employeeRepository.GetByIdentityUserIdAsync(@event.AuthenticatedUserId);
        if (authenticatedEmployee == null)
            throw new AuthenticatedUserNotFoundException("Not found employee by identity user id when consuming EmployeeCreated event");
        
        await Task.WhenAll(
            _companyRepository.IncrementEmployeeCounterAsync(@event.CompanyId, @event),
            _employeeRepository.IncrementRegistersCreatedCounter(authenticatedEmployee.Id, @event)
        );
    }
}
