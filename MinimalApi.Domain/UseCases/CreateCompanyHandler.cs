using MinimalApi.Domain.Errors;
using MinimalApi.Domain.Events;
using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Domain.UseCases.Validations;
using MinimalApi.Observability;
using MinimalApi.Utils.ValidatorUtils;

namespace MinimalApi.Domain.UseCases;

public class CreateCompanyHandler : IUseCaseHandler<CreateCompanyCmd>
{
    private readonly IValidator<CreateCompanyCmd> _cmdValidator;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEventSender _eventSender;

    public CreateCompanyHandler(IValidator<CreateCompanyCmd> cmdValidator, ICompanyRepository companyRepository, IEventSender eventSender)
    {
        _cmdValidator = cmdValidator;
        _companyRepository = companyRepository;
        _eventSender = eventSender;
    }

    public async Task<CreateCompanyCmd> ExecuteAsync(CreateCompanyCmd input)
    {
        var result = await _cmdValidator.ValidateAsync(input);
        if (!result.IsValid)
        {
            throw new InvalidInputException(result.ToMessageError());
        }

        var entity = input.ToCompanyEntity();

        entity.Validate();

        await _companyRepository.CreateAsync(entity);

        var @event = new CompanyCreated(entity.Id, input.AuthenticatedUserId);
        await _eventSender.SendEntityCreatedAsync(@event);
        
        Metrics.CompanyCreatedCounter.Add(1);

        return input with { CompanyCreated = entity };
    }
}