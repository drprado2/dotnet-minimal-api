using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.UseCases.Queries;

public record GetCompanyEmployeesQuery(Guid CompanyId, int PageSize, long? LastPageItem, IList<Employee>? EmployeesRetrieved) : UseCaseInput;