using MinimalApi.Observability;

namespace MinimalApi.WebApi.Middlewares;

public class MdcMiddleware
{
    private readonly RequestDelegate _next;

    public MdcMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Guid identity = default;
        if (!context.Request.Headers.TryGetValue("x-cid", out var cid))
        {
            cid = Guid.NewGuid().ToString();
        }
        if (context.Request.Headers.TryGetValue("x-user-id", out var identityUserId))
        {
            Guid.TryParse(identityUserId, out identity);
        }

        var mdc = new MapDiagnosticContext()
        {
            Cid = cid,
            IdentityUserId = identity == default ? null : identity
        };
        
        context.AddMdc(mdc);
        
        await _next(context);
    }
}
