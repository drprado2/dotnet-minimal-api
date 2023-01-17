using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Domain.Services;

namespace MinimalApi.Adapters.Cache;

public static class CacheServiceCollectionExtensions
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
