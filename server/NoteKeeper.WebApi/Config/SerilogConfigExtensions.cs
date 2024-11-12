using Serilog;

namespace NoteKeeper.WebApi.Config
{
    public static class SerilogConfigExtensions
    {
        public static void ConfigureSerilog(this IServiceCollection services, ILoggingBuilder logging, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithClientIp()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .WriteTo.NewRelicLogs(
                    endpointUrl: "https://log-api.newrelic.com/log/v1",
                    applicationName: "note-keeper-api",
                    licenseKey: configuration["NEW_RELIC_LICENSE_KEY"]
                )
                .CreateLogger();

            logging.ClearProviders();

            services.AddLogging(builder => builder.AddSerilog(dispose: true));
        }
    }
}
