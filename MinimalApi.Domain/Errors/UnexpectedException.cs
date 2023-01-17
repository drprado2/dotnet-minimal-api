namespace MinimalApi.Domain.Errors;

[Serializable]
public class UnexpectedException : Exception
{
    public UnexpectedException() { }

    public UnexpectedException(string message)
        : base(message) { }

    public UnexpectedException(string message, Exception inner)
        : base(message, inner) { }
}