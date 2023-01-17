using MinimalApi.Domain.Errors;

namespace MinimalApi.Domain.Events.Consumers;

public class ConsumerDispatcher : IEventConsumer<Event>
{
    private readonly IServiceProvider _serviceContainer;

    public ConsumerDispatcher(IServiceProvider serviceContainer)
    {
        _serviceContainer = serviceContainer;
    }
    
    public async Task ConsumeAsync(Event @event)
    {
        switch (@event.EventType)
        {
            case EventType.CompanyCreated:
                var c1 = _serviceContainer.GetService(typeof(IEventConsumer<CompanyCreated>)) as IEventConsumer<CompanyCreated>;
                await c1?.ConsumeAsync(@event as CompanyCreated ?? throw new InvalidOperationException());
                break;
            case EventType.EmployeeCreated:
                var c2 = _serviceContainer.GetService(typeof(IEventConsumer<EmployeeCreated>)) as IEventConsumer<EmployeeCreated>;
                await c2?.ConsumeAsync(@event as EmployeeCreated ?? throw new InvalidOperationException());
                break;
            case EventType.EmployeeEdited:
                return;
            default:
                throw new InvalidEventTypeException();
        }
    }
}
