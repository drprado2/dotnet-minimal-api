using MinimalApi.Domain.Entities.Validations;
using MinimalApi.Domain.Errors;
using MinimalApi.Utils.ValidatorUtils;

namespace MinimalApi.Domain.Entities;

public class Company : Entity
{
    private static readonly CompanyValidator Validator = new();
    
    private string _logo = DefaultLogo;
    private string _primaryColor = DefaultPrimaryColor;
    private string _primaryFontColor = DefaultPrimaryFontColor;
    private string _secondaryColor = DefaultSecondaryColor;
    private string _secondaryFontColor = DefaultSecondaryFontColor;
    private const string DefaultLogo = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fpng.pngtree.com%2Felement_our%2Fpng%2F20180912%2Fcoffee-time-png_91570.jpg&imgrefurl=https%3A%2F%2Fpt.pngtree.com%2Ffree-logo-png&tbnid=Np5bSfjgPteq4M&vet=12ahUKEwjKuq7Apan8AhWBNbkGHT1NDqYQMygDegUIARDDAQ..i&docid=h4myd2GHM0FmOM&w=360&h=360&q=logo%20png&ved=2ahUKEwjKuq7Apan8AhWBNbkGHT1NDqYQMygDegUIARDDAQ";
    private const string DefaultPrimaryColor = "#000066";
    private const string DefaultPrimaryFontColor = "#cce4ff";
    private const string DefaultSecondaryColor = "#ffffff";
    private const string DefaultSecondaryFontColor = "#222222";
    
    public string Name { get; set; } = default!;
    public ValueObjects.Document Document { get; set; } = default!;
    
    public string DocumentValue => Document.Value;

    public string Logo
    {
        get => _logo;
        set 
        {
            if (!string.IsNullOrEmpty(value))
            {
                _logo = value;
            }
        }
    }

    public string PrimaryColor
    {
        get => _primaryColor;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _primaryColor = value;
            }
        }
    }

    public string PrimaryFontColor
    {
        get => _primaryFontColor;
        set 
        {
            if (!string.IsNullOrEmpty(value))
            {
                _primaryFontColor = value;
            }
        }
    }

    public string SecondaryColor
    {
        get => _secondaryColor;
        set 
        {
            if (!string.IsNullOrEmpty(value))
            {
                _secondaryColor = value;
            }
        }
    }

    public string SecondaryFontColor
    {
        get => _secondaryFontColor;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _secondaryFontColor = value;
            }
        }
    }

    public long TotalCollaborators { get; set; } = 0;
    public bool Active { get; set; } = true;
    
    public override void Validate()
    {
        var result = Validator.Validate(this);
        if (!result.IsValid)
        {
            throw new InvalidCompanyException(result.ToMessageError());
        }
    }
}
