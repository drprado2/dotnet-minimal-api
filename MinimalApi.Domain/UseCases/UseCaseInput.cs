using MinimalApi.Observability;

namespace MinimalApi.Domain.UseCases;

public abstract record UseCaseInput
{
    public MapDiagnosticContext Mdc { get; init; }
}
