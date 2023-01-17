namespace MinimalApi.Domain.Services;

public interface ICacheService
{
    public Task<T?> GetOrDefaultAsync<T>(string key);
    public Task PutAsync<T>(string key, T input, TimeSpan expiration);
}
