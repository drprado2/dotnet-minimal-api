using MinimalApi.Observability;

namespace MinimalApi.WebApi.Middlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogMiddleware> _logger;

    public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
    {
        _next = next;
        // _logger = loggerFactory.CreateLogger<LogMiddleware>();
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var mdc = context.GetMdc();

        var p = mdc.LogArgs;
        _logger.LogInformation($"Handling request {context.Request.Method} - {context.Request.Path}", p);
        _logger.LogInformation($"Another", 1, "test", 2, "test2");
        
        await _next(context);
    }

}
