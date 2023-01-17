namespace MinimalApi.Domain.Errors;

[Serializable]
public class InvalidEmployeeException : BusinessException
{
    public InvalidEmployeeException() { }

    public InvalidEmployeeException(string message)
        : base(message) { }

    public InvalidEmployeeException(string message, Exception inner)
        : base(message, inner) { }
}