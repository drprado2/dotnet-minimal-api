namespace MinimalApi.Domain.Errors;

[Serializable]
public class AuthenticatedUserNotFoundException : BusinessException
{
    public AuthenticatedUserNotFoundException() { }

    public AuthenticatedUserNotFoundException(string message)
        : base(message) { }

    public AuthenticatedUserNotFoundException(string message, Exception inner)
        : base(message, inner) { }

}
