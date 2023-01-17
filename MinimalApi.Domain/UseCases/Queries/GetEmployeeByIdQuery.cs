using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.UseCases.Queries;

public record GetEmployeeByIdQuery(Guid EmployeeId, Employee? EmployeeRetrieved) : UseCaseInput;