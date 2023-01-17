using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace MinimalApi.Adapters.Storage.SqlServer;

public abstract class BaseRepository
{
    private readonly DbProviderFactory _dbProviderFactory;
    private readonly string _connectionString;
    private const string MinimalApiDbName = "MinimalApi";
    

    public BaseRepository(DbProviderFactory dbProviderFactory, IConfiguration config)
    {
        _dbProviderFactory = dbProviderFactory;
        _connectionString = config.GetConnectionString(MinimalApiDbName) ?? "";
    }

    protected DbConnection? CreateConnection()
    {
        var db = _dbProviderFactory.CreateConnection();
        db.ConnectionString = _connectionString;
        return db;
    }
}
