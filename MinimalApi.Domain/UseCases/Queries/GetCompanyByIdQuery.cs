using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.UseCases.Queries;

public record GetCompanyByIdQuery(Guid CompanyId, Company? CompanyRetrieved) : UseCaseInput;