namespace MinimalApi.Domain.Events;

public interface IEventSender
{
    public Task SendEntityCreatedAsync<TEvent>(TEvent @event) where TEvent : Event;
    public Task SendEntityUpdatedAsync<TEvent>(TEvent @event) where TEvent : Event;
}