namespace MinimalApi.Domain.Errors;

[Serializable]
public class InvalidCompanyException : BusinessException
{
    public InvalidCompanyException() { }

    public InvalidCompanyException(string message)
        : base(message) { }

    public InvalidCompanyException(string message, Exception inner)
        : base(message, inner) { }
}