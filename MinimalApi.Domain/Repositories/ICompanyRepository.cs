using System.Data;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Events;

namespace MinimalApi.Domain.Repositories;

public interface ICompanyRepository
{
    public Task CreateAsync(Company company, IDbTransaction? dbTransaction=null);
    public Task<Company?> GetByIdAsync(Guid id);
    public Task<bool> CompanyExistsAsync(Guid id);
    public Task IncrementEmployeeCounterAsync(Guid id, EmployeeCreated? @event=null);
}
