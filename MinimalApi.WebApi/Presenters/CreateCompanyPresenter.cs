using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Observability;

namespace MinimalApi.WebApi.Presenters;

public static class CreateCompanyPresenter
{
    public static CreateCompanyCmd ToUseCaseInput(Guid? authenticatedUserId, CreateCompanyInput input, MapDiagnosticContext mdc)
    {
        return new CreateCompanyCmd(authenticatedUserId, input.Document, input.Name, input.Logo, input.PrimaryColor, input.PrimaryFontColor, input.SecondaryColor, input.SecondaryFontColor,
            null)
        {
            Mdc = mdc
        };
    }

    public static CreateCompanyOutput ToOutput(CreateCompanyCmd cmd)
    {
        if (cmd == null || cmd.CompanyCreated == null)
            throw new NullReferenceException();
        
        return new CreateCompanyOutput
        {
            Id = cmd.CompanyCreated.Id,
            Document = cmd.CompanyCreated.Document.Value,
            Name = cmd.CompanyCreated.Name,
            Logo = cmd.CompanyCreated.Logo,
            PrimaryColor = cmd.CompanyCreated.PrimaryColor,
            PrimaryFontColor = cmd.CompanyCreated.PrimaryFontColor,
            SecondaryColor = cmd.CompanyCreated.SecondaryColor,
            SecondaryFontColor = cmd.CompanyCreated.SecondaryFontColor,
            Active = cmd.CompanyCreated.Active,
            TotalCollaborators = cmd.CompanyCreated.TotalCollaborators,
            CreatedAt = cmd.CompanyCreated.CreatedAt,
        };
    }
}



public struct CreateCompanyInput
{
    public string Document { get; set; }
    public string Name { get; set; }
    public string? Logo { get; set; }
    public string? PrimaryColor { get; set; }
    public string? PrimaryFontColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? SecondaryFontColor { get; set; }
}

public struct CreateCompanyOutput
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
}
