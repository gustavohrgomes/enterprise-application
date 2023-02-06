using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace NSE.Core.Logging;

public static class LoggerConfigurationExtensions
{
    public static void Configure(this LoggerConfiguration loggerConfiguration, HostBuilderContext context)
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration, nameof(loggerConfiguration));

        ArgumentNullException.ThrowIfNull(context, nameof(context));

        loggerConfiguration
            .Enrich.WithProperty("Host", Environment.MachineName)
            .Enrich.WithProperty("OS", Environment.OSVersion)
            .Enrich.WithProperty("Service", context.Configuration["ApplicationName"])
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["Elasticsearch:Url"]))
            {
                IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            }) 
            .ReadFrom.Configuration(context.Configuration);
    }
}
