namespace MinimalApi.Domain.Events;

public interface IEventFactory
{
    public Event? Build(string eventType, byte[] eventPayload);
}
