using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Events;
using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.ValueObjects;

namespace MinimalApi.Adapters.Storage.SqlServer;

public class CompanyRepository : BaseRepository, ICompanyRepository
{
    public CompanyRepository(DbProviderFactory dbProviderFactory, IConfiguration config) : base(dbProviderFactory, config)
    {
    }

    public async Task CreateAsync(Company company, IDbTransaction? dbTransaction = null)
    {
        const string query = """
INSERT INTO Company (CompanyID, Name, Document, Logo, PrimaryColor, PrimaryFontColor, SecondaryColor, SecondaryFontColor, Active, CreatedAt, UpdatedAt) 
VALUES (@Id, @Name, @DocumentValue, @Logo, @PrimaryColor, @PrimaryFontColor, @SecondaryColor, @SecondaryFontColor, @Active, @CreatedAt, @UpdatedAt)
""";

        await using var db = CreateConnection();
        await db.ExecuteAsync(query, company, dbTransaction);
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        const string query = """
SELECT CompanyID, Name, Document, Logo, PrimaryColor, PrimaryFontColor, SecondaryColor, SecondaryFontColor, Active, CreatedAt, UpdatedAt, TotalCollaborators, InternalId, xmin as DataVersion  
FROM Company WHERE CompanyID = @id
""";

        await using var db = CreateConnection();
        var result = await db.QueryFirstOrDefaultAsync(query, new { id = id });
        if (result == null)
            return null;

        return new Company()
        {
            Active = result.active,
            Id = result.companyid,
            Name = result.name,
            Document = new Document(DocumentType.CNPJ, result.document),
            Logo = result.logo,
            PrimaryColor = result.primarycolor,
            PrimaryFontColor = result.primaryfontcolor,
            SecondaryColor = result.secondarycolor,
            SecondaryFontColor = result.secondaryfontcolor,
            CreatedAt = result.createdat,
            UpdatedAt = result.updatedat,
            TotalCollaborators = result.totalcollaborators,
            InternalId = result.internalid,
            DataVersion = result.dataversion
        };
    }

    public async Task<bool> CompanyExistsAsync(Guid id)
    {
        const string query = """
SELECT 1  
FROM Company WHERE CompanyID = @id LIMIT 1
""";

        await using var db = CreateConnection();
        var result = await db.QueryFirstOrDefaultAsync<bool>(query, new { id = id });
        return result;
    }

    public async Task IncrementEmployeeCounterAsync(Guid id, EmployeeCreated? @event = null)
    {
        const string incrementQuery = """
UPDATE Company SET TotalCollaborators = TotalCollaborators + 1 WHERE CompanyID = @id;
""";

        const string addUniqueEventQuery = """
INSERT INTO UniqueEvents (EventId, DateConsumed, EventType, ConsumerAction) VALUES (@eventId, @dateConsumed, @eventType, @consumerAction)
""";

        const string consumeEventAction = "IncrementCompanyEmployeeCounter";

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
            await db.ExecuteAsync(addUniqueEventQuery,
                new { eventId = @event.Id, dateConsumed = DateTime.Now, eventType = @event.EventType.ToString(), consumerAction = consumeEventAction }, tx);
            await db.ExecuteAsync(incrementQuery, new { id }, tx);
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
