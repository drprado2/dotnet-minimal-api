using MinimalApi.Domain.Errors;
using MinimalApi.Domain.Repositories;

namespace MinimalApi.Domain.Events.Consumers;

public class CompanyCreatedConsumer : IEventConsumer<CompanyCreated>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CompanyCreatedConsumer(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task ConsumeAsync(CompanyCreated @event)
    {
        if (@event.AuthenticatedUserId == null)
            return;
        
        var authenticatedEmployee = await  _employeeRepository.GetByIdentityUserIdAsync(@event.AuthenticatedUserId.Value);
        if (authenticatedEmployee == null)
            throw new AuthenticatedUserNotFoundException("Not found employee by identity user id when consuming CompanyCreated event");

        await _employeeRepository.IncrementRegistersCreatedCounter(authenticatedEmployee.Id, @event);
    }
}
