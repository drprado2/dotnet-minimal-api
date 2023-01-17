using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Observability;

namespace MinimalApi.WebApi.Presenters;

public static class GetEmployeeByIdPresenter
{
    public static GetEmployeeByIdQuery ToUseCaseInput(Guid employeeId, MapDiagnosticContext mdc)
    {
        return new GetEmployeeByIdQuery(employeeId, null);
    }

    public static GetEmployeeByIdOutput ToOutput(GetEmployeeByIdQuery query)
    {
        if (query == null || query.EmployeeRetrieved == null)
            throw new NullReferenceException();
        
        return new GetEmployeeByIdOutput
        {
            Id = query.EmployeeRetrieved.Id,
            Name = query.EmployeeRetrieved.Name,
            Email = query.EmployeeRetrieved.Email,
            Phone = query.EmployeeRetrieved.Phone,
            BirthDate = query.EmployeeRetrieved.BirthDate,
            IdentityUserId = query.EmployeeRetrieved.IdentityUserId,
            Active = query.EmployeeRetrieved.Active,
            CreatedAt = query.EmployeeRetrieved.CreatedAt,
            InternalId = query.EmployeeRetrieved.InternalId,
            DataVersion = query.EmployeeRetrieved.DataVersion,
            CompanyId = query.EmployeeRetrieved.CompanyId,
            RecordCreatedCount = query.EmployeeRetrieved.RecordCreatedCount,
            RecordEditedCount = query.EmployeeRetrieved.RecordEditedCount,
            RecordDeletedCount = query.EmployeeRetrieved.RecordDeletedCount,
        };
    }
}

public struct GetEmployeeByIdOutput
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
    public int RecordCreatedCount { get; set; }
    public int RecordEditedCount { get; set; }
    public int RecordDeletedCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long InternalId { get; set; }
    public byte[]? DataVersion { get; set; }
}
