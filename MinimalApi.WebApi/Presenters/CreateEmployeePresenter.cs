using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Observability;

namespace MinimalApi.WebApi.Presenters;

public static class CreateEmployeePresenter
{
    public static CreateEmployeeCmd ToUseCaseInput(Guid? authenticatedUserId, CreateEmployeeInput input, MapDiagnosticContext mdc)
    {
        return new CreateEmployeeCmd(authenticatedUserId ?? default, input.CompanyId ?? default, input.IdentityUserId ?? default, input.Name, input.Email, input.Phone, input.BirthDate ?? default, null)
        {
            Mdc = mdc
        };
    }

    public static CreateEmployeeOutput ToOutput(CreateEmployeeCmd cmd)
    {
        if (cmd == null || cmd.EmployeeCreated == null)
            throw new NullReferenceException();

        return new CreateEmployeeOutput
        {
            Id = cmd.EmployeeCreated.Id,
            Name = cmd.EmployeeCreated.Name,
            Email = cmd.EmployeeCreated.Email,
            Phone = cmd.EmployeeCreated.Phone,
            BirthDate = cmd.EmployeeCreated.BirthDate,
            IdentityUserId = cmd.EmployeeCreated.IdentityUserId,
            Active = cmd.EmployeeCreated.Active,
            CreatedAt = cmd.EmployeeCreated.CreatedAt,
            CompanyId = cmd.EmployeeCreated.CompanyId,
            RecordCreatedCount = cmd.EmployeeCreated.RecordCreatedCount,
            RecordEditedCount = cmd.EmployeeCreated.RecordEditedCount,
            RecordDeletedCount = cmd.EmployeeCreated.RecordDeletedCount,
        };
    }
}

public struct CreateEmployeeInput
{
    public Guid? CompanyId { get; set; }
    public Guid? IdentityUserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
}

public struct CreateEmployeeOutput
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
}
