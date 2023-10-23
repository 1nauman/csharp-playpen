using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace otel_playpen.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOTel(this IServiceCollection services, Action<ResourceBuilder> configureResource)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter();
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter();
            });
        
        return services;
    }
}