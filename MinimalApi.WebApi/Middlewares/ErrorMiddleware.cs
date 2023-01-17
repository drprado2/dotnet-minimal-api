using MinimalApi.Domain.Errors;
using MinimalApi.WebApi.Presenters;

namespace MinimalApi.WebApi.Middlewares;

/// <summary>
/// Global middleware for handling exceptions
/// To use custom logic for specific endpoint use EndpointFilters
/// </summary>
public class ErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(BusinessException ex)
        {
            _logger.LogWarning(ex, "Business exception");
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToBusinessError(ex.Message));
        }
        catch(InvalidInputException ex)
        {
            _logger.LogWarning(ex, "Invalid input exception");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToPayloadError(ex.Message));
        }
        catch(BadHttpRequestException ex)
        {
            _logger.LogWarning(ex, "Bad request exception");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToPayloadError(ex.Message));
        }
        catch(UnexpectedException ex)
        {
            _logger.LogError(ex, "Unexpected exception");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToUnexpectedError());
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToUnexpectedError());
        }
    }
}
