using System.Diagnostics;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Repositories;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Observability;

namespace MinimalApi.Domain.UseCases;

public class GetCompanyByIdHandler : IUseCaseHandler<GetCompanyByIdQuery>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICacheService _cacheService;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    public GetCompanyByIdHandler(ICompanyRepository companyRepository, ICacheService cacheService)
    {
        _companyRepository = companyRepository;
        _cacheService = cacheService;
    }

    public async Task<GetCompanyByIdQuery> ExecuteAsync(GetCompanyByIdQuery input)
    {
        using var activity = AppObservability.ActivitySource.StartActivity(ActivityKind.Server);
        
        var company = await _cacheService.GetOrDefaultAsync<Company>(input.CompanyId.ToString());
        if (company == default)
        {
            company = await _companyRepository.GetByIdAsync(input.CompanyId);
            if (company != default)
                await _cacheService.PutAsync(input.CompanyId.ToString(), company, CacheDuration);
        }

        if (company != default)
        {
            activity
                .SetTag("CompanyName", company.Name)
                .SetTag("CompanyLogo", company.Logo);
        }
        
        return input with { CompanyRetrieved = company};
    }
}
