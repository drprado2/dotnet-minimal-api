using System.Transactions;

namespace MinimalApi.Domain.Events;

public record EmployeeCreated : Event
{
    public EmployeeCreated(Guid companyId, Guid authenticatedUserId, Guid employeeId)
    {
        this.EventType = EventType.EmployeeCreated;
        this.CompanyId = companyId;
        this.AuthenticatedUserId = authenticatedUserId;
        this.EmployeeId = employeeId;
    }
    
    public Guid CompanyId { get; set; }
    public Guid AuthenticatedUserId { get; set; }
    public Guid EmployeeId { get; set; }
};
