namespace MinimalApi.WebApi.Presenters;

public static class ErrorPresenter
{
    private const string UnexpectedErrorMessage = "An unexpected error occurred.";
    private const string UnexpectedErrorCode = "UnexpectedError";
    private const string BusinessErrorCode = "BusinessError";
    private const string PayloadErrorCode = "PayloadError";
    
    
    public static Error ToUnexpectedError()
    {
        return new Error
        {
            Code = UnexpectedErrorCode,
            Message = UnexpectedErrorMessage
        };
    }
    
    public static Error ToBusinessError(string error)
    {
        return new Error
        {
            Code = BusinessErrorCode,
            Message = error
        };
    }
    
    public static Error ToPayloadError(string error)
    {
        return new Error
        {
            Code = PayloadErrorCode,
            Message = error
        };
    }
}

public struct Error
{
    // If you wish to create catalog of errors, you can use this property to identify the error
    public string? Code { get; set; }
    public string Message { get; set; }
}
