namespace MinimalApi.Domain.Errors;

[Serializable]
public class InvalidEventTypeException : BusinessException
{
    public InvalidEventTypeException() { }

    public InvalidEventTypeException(string message)
        : base(message) { }

    public InvalidEventTypeException(string message, Exception inner)
        : base(message, inner) { }
}
