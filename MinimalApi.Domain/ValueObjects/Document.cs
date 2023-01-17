namespace MinimalApi.Domain.ValueObjects;

public enum DocumentType
{
    CNPJ,
}

public class Document
{
    public Document()
    {
    }
    public Document(DocumentType documentType, string value)
    {
        Type = documentType;
        Value = value;
    }
    
    public DocumentType Type { get; init; }
    public string Value { get; init; } = default!;

    public bool Validate() =>
        Type switch
        {
            DocumentType.CNPJ => ValidateCnpj(),
            _ => false
        };
    
    private bool ValidateCnpj() 
    {
        int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        if (Value.Length != 14)
            return false;

        string tempCnpj = Value.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCnpj = tempCnpj + digito;
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return Value.EndsWith(digito);
    }
}