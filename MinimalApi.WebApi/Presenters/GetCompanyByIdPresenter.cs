using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Observability;

namespace MinimalApi.WebApi.Presenters;

public static class GetCompanyByIdPresenter
{
    public static GetCompanyByIdQuery ToUseCaseInput(Guid companyId, MapDiagnosticContext mdc)
    {
        return new GetCompanyByIdQuery(companyId, null)
        {
            Mdc = mdc
        };
    }

    public static GetCompanyByIdOutput ToOutput(GetCompanyByIdQuery query)
    {
        if (query == null || query.CompanyRetrieved == null)
            throw new NullReferenceException();
        
        return new GetCompanyByIdOutput
        {
            Id = query.CompanyRetrieved.Id,
            Document = query.CompanyRetrieved.Document.Value,
            Name = query.CompanyRetrieved.Name,
            Logo = query.CompanyRetrieved.Logo,
            PrimaryColor = query.CompanyRetrieved.PrimaryColor,
            PrimaryFontColor = query.CompanyRetrieved.PrimaryFontColor,
            SecondaryColor = query.CompanyRetrieved.SecondaryColor,
            SecondaryFontColor = query.CompanyRetrieved.SecondaryFontColor,
            Active = query.CompanyRetrieved.Active,
            TotalCollaborators = query.CompanyRetrieved.TotalCollaborators,
            CreatedAt = query.CompanyRetrieved.CreatedAt,
            UpdateAt = query.CompanyRetrieved.UpdatedAt,
            InternalId = query.CompanyRetrieved.InternalId,
            DataVersion = query.CompanyRetrieved.DataVersion,
        };
    }
}

public struct GetCompanyByIdOutput
{
    public Guid Id { get; set; }
    public string Document { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public string PrimaryColor { get; set; }
    public string PrimaryFontColor { get; set; }
    public string SecondaryColor { get; set; }
    public string SecondaryFontColor { get; set; }
    public bool Active { get; set; }
    public long TotalCollaborators { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public long InternalId { get; set; }
    public byte[] DataVersion { get; set; }
}
