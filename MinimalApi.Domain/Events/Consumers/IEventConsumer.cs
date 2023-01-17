namespace MinimalApi.Domain.Events.Consumers;

public interface IEventConsumer<T> where T : Event
{
    public Task ConsumeAsync(T @event);
}
