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

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(BusinessException ex)
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToBusinessError(ex.Message));
        }
        catch(InvalidInputException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToPayloadError(ex.Message));
        }
        catch(BadHttpRequestException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToPayloadError(ex.Message));
        }
        catch(UnexpectedException ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToUnexpectedError());
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ErrorPresenter.ToUnexpectedError());
        }
    }
}
