namespace MinimalApi.Domain.UseCases;

public interface IUseCaseHandler<T> where T : notnull
{
    public Task<T> ExecuteAsync(T input);
}