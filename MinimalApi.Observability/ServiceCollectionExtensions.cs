using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace MinimalApi.Observability;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                    opt.ExportProcessorType = ExportProcessorType.Batch;
                })
                .AddSource(AppObservability.AppName)
                .SetResourceBuilder(AppObservability.OpenTelemetryResourceBuilder)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddRedisInstrumentation()
                .AddSqlClientInstrumentation();
        });

        return services;
    }
    
    public static IServiceCollection AddMetrics(this IServiceCollection services)
    {
        services.AddOpenTelemetryMetrics(metricProviderBuilder =>
        {
            metricProviderBuilder
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                    opt.ExportProcessorType = ExportProcessorType.Batch;
                })
                .AddMeter(AppObservability.AppName)
                .SetResourceBuilder(AppObservability.OpenTelemetryResourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
        });

        return services;
    }
    
    public static WebApplicationBuilder AddAppLogging(this WebApplicationBuilder appBuilder)
    {
        appBuilder.Logging.ClearProviders();
        
        appBuilder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(AppObservability.OpenTelemetryResourceBuilder);
            if (appBuilder.Environment.IsDevelopment())
            {
                // options.AddConsoleExporter();
            }

            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                otlpOptions.ExportProcessorType = ExportProcessorType.Batch;
            });
        });

        return appBuilder;
    }
}
