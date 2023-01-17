using System.Text.Json;
using MinimalApi.Domain.Errors;

namespace MinimalApi.Domain.Events;

public class EventFactory : IEventFactory
{
    public Event? Build(string eventType, byte[] eventPayload)
    {
        EventType evType;
        if (!Enum.TryParse(eventType, true, out evType))
            throw new InvalidEventTypeException($"Event type ${eventType} is not recognized");

        switch (evType)
        {
            case EventType.CompanyCreated:
                return JsonSerializer.Deserialize<CompanyCreated>(eventPayload);
            case EventType.EmployeeCreated:
                return JsonSerializer.Deserialize<EmployeeCreated>(eventPayload);
            case EventType.EmployeeEdited:
                return JsonSerializer.Deserialize<EmployeeEdited>(eventPayload);
            default:
                throw new InvalidEventTypeException($"There is no builder to the event ${eventType}");
        }
    }
}
