using Microsoft.AspNetCore.Http;

namespace MinimalApi.Observability;

public static class ObservabilityExtensions
{
    public static MapDiagnosticContext GetMdc(this HttpContext context)
    {
        var item = context.Items[AppObservability.ObservabilityMdcKey];
        return item as MapDiagnosticContext ?? new MapDiagnosticContext();
    }
    
    public static void AddMdc(this HttpContext context, MapDiagnosticContext mdc)
    {
        context.Items[AppObservability.ObservabilityMdcKey] = mdc;
    }
}
