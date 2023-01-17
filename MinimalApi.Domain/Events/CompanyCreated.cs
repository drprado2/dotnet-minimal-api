namespace MinimalApi.Domain.Events;

public record CompanyCreated : Event
{
    public CompanyCreated(Guid companyId, Guid? authenticatedUserId)
    {
        this.EventType = EventType.CompanyCreated;
        CompanyId = companyId;
        AuthenticatedUserId = authenticatedUserId;
    }
    
    public Guid CompanyId { get; set; }
    public Guid? AuthenticatedUserId { get; set; }
};