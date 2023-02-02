using Microsoft.Extensions.Configuration;
using Serilog;

namespace NSE.Core.Logging;

public static class LoggerConfigurationExtensions
{
    public static void Configure(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration, nameof(loggerConfiguration));

        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        loggerConfiguration
            .Enrich.WithProperty("Host", Environment.MachineName)
            .Enrich.WithProperty("OS", Environment.OSVersion)
            .ReadFrom.Configuration(configuration);
    }
}
