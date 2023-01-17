namespace MinimalApi.Domain.Events;

public abstract record Event
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public EventType EventType { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public string CorrelationId { get; set; } = default!;
    public string UniqueKey => GetType().ToString();
}