using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Domain.Repositories;

namespace MinimalApi.Adapters.Storage.SqlServer;

public static class SqlServerServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ICompanyRepository, CompanyRepository>();
        services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

        return services;
    }

}
