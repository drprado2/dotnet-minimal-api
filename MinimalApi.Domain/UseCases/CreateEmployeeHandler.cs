using System.Diagnostics.Metrics;
using MinimalApi.Domain.Errors;
using MinimalApi.Domain.Events;
using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Domain.UseCases.Validations;
using MinimalApi.Observability;
using MinimalApi.Utils.ValidatorUtils;

namespace MinimalApi.Domain.UseCases;

public class CreateEmployeeHandler : IUseCaseHandler<CreateEmployeeCmd>
{
    private readonly IValidator<CreateEmployeeCmd> _cmdValidator;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEventSender _eventSender;

    public CreateEmployeeHandler(IValidator<CreateEmployeeCmd> cmdValidator, IEmployeeRepository employeeRepository, IEventSender eventSender)
    {
        _cmdValidator = cmdValidator;
        _employeeRepository = employeeRepository;
        _eventSender = eventSender;
    }

    public async Task<CreateEmployeeCmd> ExecuteAsync(CreateEmployeeCmd input)
    {
        var result = await _cmdValidator.ValidateAsync(input);
        if (!result.IsValid)
        {
            throw new InvalidInputException(result.ToMessageError());
        }

        var entity = input.ToEmployeeEntity();

        entity.Validate();

        await _employeeRepository.CreateAsync(entity);
        
        var @event = new EmployeeCreated(entity.CompanyId, input.AuthenticatedUserId, entity.Id);
        await _eventSender.SendEntityCreatedAsync(@event);
        
        Metrics.EmployeeCreatedCounter.Add(1);

        return input with { EmployeeCreated = entity };
    }
}
