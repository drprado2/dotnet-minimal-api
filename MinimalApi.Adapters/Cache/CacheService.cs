using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MinimalApi.Domain.Services;
using StackExchange.Redis;

namespace MinimalApi.Adapters.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _redisCache;

    public CacheService(IConnectionMultiplexer redisCache)
    {
        _redisCache = redisCache.GetDatabase();
    }
    
    public async Task<T?> GetOrDefaultAsync<T>(string key)
    {
        var value = await _redisCache.StringGetAsync(key);
        // var value = await _redisCache.GetStringAsync(key);
        return !value.HasValue ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task PutAsync<T>(string key, T input, TimeSpan expiration)
    {
        await _redisCache.StringSetAsync(key, JsonSerializer.Serialize(input), expiration);
        // await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(input), new DistributedCacheEntryOptions
        // {
        //     AbsoluteExpirationRelativeToNow = expiration
        // });
    }
}
