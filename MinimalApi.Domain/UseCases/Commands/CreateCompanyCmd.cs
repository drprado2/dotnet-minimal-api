using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ValueObjects;

namespace MinimalApi.Domain.UseCases.Commands;

public record CreateCompanyCmd(Guid? AuthenticatedUserId, string Document, string Name, string? Logo, string? PrimaryColor,
    string? PrimaryFontColor, string? SecondaryColor, string? SecondaryFontColor, Company? CompanyCreated) : UseCaseInput
{
    public Company ToCompanyEntity()
    {
        return new Company()
        {
            Document = new Document(DocumentType.CNPJ, Document),
            Name = Name,
            Logo = Logo,
            PrimaryColor = PrimaryColor,
            PrimaryFontColor = PrimaryFontColor,
            SecondaryColor = SecondaryColor,
            SecondaryFontColor = SecondaryFontColor
        };
    }
};