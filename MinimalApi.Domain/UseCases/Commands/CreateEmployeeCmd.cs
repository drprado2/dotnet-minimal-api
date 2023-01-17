using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.UseCases.Commands;

public record CreateEmployeeCmd(Guid AuthenticatedUserId, Guid CompanyId, Guid IdentityUserId, string Name, string Email, string Phone, DateTime BirthDate, Employee? EmployeeCreated) : UseCaseInput
{
    public Employee ToEmployeeEntity()
    {
        return new Employee()
        {
            IdentityUserId = IdentityUserId,
            CompanyId = CompanyId,
            Name = Name,
            Email = Email,
            Phone = Phone,
            BirthDate = BirthDate
        };
    }
};