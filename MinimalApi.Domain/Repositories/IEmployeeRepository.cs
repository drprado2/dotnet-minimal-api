using System.Data;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Events;

namespace MinimalApi.Domain.Repositories;

public interface IEmployeeRepository
{
    public Task CreateAsync(Employee company, IDbTransaction? dbTransaction = null);
    public Task<Employee> GetByIdAsync(Guid id);
    public Task<IEnumerable<Employee>> GetEmployeesFromCompanyAsync(Guid companyId, int pageSize, long? lastPageId);
    public Task IncrementRegistersCreatedCounter(Guid id, Event? @event=null);
    Task<Employee> GetByIdentityUserIdAsync(Guid identityUserId);
}
