using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Resources;

namespace MinimalApi.Observability;


public class AppObservability
{
    public static readonly String AppName = Environment.GetEnvironmentVariable("ASPNETCORE_APPNAME") ?? "MinimalApi";
    public static readonly String AppVersion = Environment.GetEnvironmentVariable("ASPNETCORE_APPVERSION") ?? "1.0.0";
    
    public static ResourceBuilder OpenTelemetryResourceBuilder = ResourceBuilder.CreateDefault()
        .AddService(serviceName: AppName, serviceVersion: AppVersion);

    public static ActivitySource ActivitySource = new ActivitySource(AppName, AppVersion);
    
    public static Meter AppMeter = new Meter(AppName, AppVersion);
    
    public const string ObservabilityMdcKey = "ObservabilityMDC";


}
