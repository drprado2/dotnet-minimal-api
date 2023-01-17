namespace MinimalApi.Domain.Events;

public record EmployeeEdited(Guid EmployeeId, Guid AuthenticatedUserId, string PreviousValueJson, string NewValueJson) : Event;