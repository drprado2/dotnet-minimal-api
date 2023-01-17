using MinimalApi.Domain.Entities.Validations;
using MinimalApi.Domain.Errors;
using MinimalApi.Utils.ValidatorUtils;

namespace MinimalApi.Domain.Entities;

public class Employee : Entity
{
    private static readonly EmployeeValidator Validator = new();
    
    public Guid IdentityUserId { get; set; }
    public Guid CompanyId { get; init; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; } = true;
    public int RecordCreatedCount { get; set; }
    public int RecordEditedCount { get; set; }
    public int RecordDeletedCount { get; set; }
    
    public override void Validate()
    {
        var result = Validator.Validate(this);
        if (!result.IsValid)
        {
            throw new InvalidEmployeeException(result.ToMessageError());
        }
    }
}