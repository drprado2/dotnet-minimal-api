using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Events;
using MinimalApi.Domain.Repositories;

namespace MinimalApi.Adapters.Storage.SqlServer;

public class EmployeeRepository : BaseRepository, IEmployeeRepository
{
    public EmployeeRepository(DbProviderFactory dbProviderFactory, IConfiguration config) : base(dbProviderFactory, config)
    {
    }

    public async Task CreateAsync(Employee employee, IDbTransaction? dbTransaction = null)
    {
        const string query = """
INSERT INTO Employee (EmployeeID, IdentityUserId, CompanyId, Name, Email, Phone, BirthDate, Active, CreatedAt, UpdatedAt) 
VALUES (@Id, @IdentityUserId, @CompanyId, @Name, @Email, @Phone, @BirthDate, @Active, @CreatedAt, @UpdatedAt)
""";

        await using var db = CreateConnection();
        await db.ExecuteAsync(query, employee, dbTransaction);
    }

    public async Task<Employee> GetByIdAsync(Guid id)
    {
        const string query = """
SELECT EmployeeID as Id, IdentityUserId, CompanyId, Name, Email, Phone, BirthDate, Active, CreatedAt, UpdatedAt, RecordCreatedCount, RecordEditedCount, RecordDeletedCount, InternalId, DataVersion
FROM Employee WHERE EmployeeID = @id
""";

        await using var db = CreateConnection();
        return await db.QueryFirstOrDefaultAsync<Employee>(query, new { id = id });
    }
    
    public async Task<Employee> GetByIdentityUserIdAsync(Guid identityUserId)
    {
        const string query = """
SELECT EmployeeID as Id, IdentityUserId, CompanyId, Name, Email, Phone, BirthDate, Active, CreatedAt, UpdatedAt, RecordCreatedCount, RecordEditedCount, RecordDeletedCount, InternalId, DataVersion
FROM Employee WHERE IdentityUserId = @id
""";

        await using var db = CreateConnection();
        return await db.QueryFirstOrDefaultAsync<Employee>(query, new { id = identityUserId });
    }

    public async Task<IEnumerable<Employee>> GetEmployeesFromCompanyAsync(Guid companyId, int pageSize, long? lastPageId=long.MaxValue)
    {
        const string query = """
SELECT TOP (@pageSize) EmployeeID as Id, Name, Email, Active, CreatedAt, DataVersion, InternalId FROM Employee WHERE InternalId < @lastPageId AND CompanyId = @companyId order by InternalId desc;
""";

        await using var db = CreateConnection();
        return await db.QueryAsync<Employee>(query, new { pageSize = pageSize, lastPageId = lastPageId ?? long.MaxValue, companyId });
    }

    public async Task IncrementRegistersCreatedCounter(Guid id, Event? @event = null)
    {
        const string incrementQuery = """
UPDATE Employee SET RecordCreatedCount = RecordCreatedCount + 1 WHERE EmployeeID = @Id;
""";
        
        const string addUniqueEventQuery = """
INSERT INTO UniqueEvents (EventId, DateConsumed, EventType, ConsumerAction) VALUES (@eventId, @dateConsumed, @eventType, @consumerAction)
""";

        const string consumeEventAction = "IncrementEmployeeRegistersCreatedCounter";
        
        await using var db = CreateConnection();
        
        if (@event == null)
        {
            await db.ExecuteAsync(incrementQuery, new { id });
            return;
        }

        await db.OpenAsync();
        await using var tx = await db.BeginTransactionAsync();

        try
        {
            await db.ExecuteAsync(addUniqueEventQuery, new {eventId = @event.Id, dateConsumed = DateTime.Now, eventType = @event.EventType.ToString(), consumerAction = consumeEventAction}, tx);
            await db.ExecuteAsync(incrementQuery, new {id}, tx);
            await tx.CommitAsync();
        }
        catch (SqlException ex)
        {
            await tx.RollbackAsync();
            
            const int duplicateKey = 2627;
            if (ex.Number == duplicateKey)
                return;

            throw;
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
