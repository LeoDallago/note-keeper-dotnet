using Serilog;

namespace NoteKeeper.WebApi.Config
{
    public static class SerilogConfigExtensions
    {
        public static void ConfigureSerilog(this IServiceCollection services, ILoggingBuilder logging)
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
                    licenseKey: "eea18f604e6014dff551fedd6f5346d0FFFFNRAL"
                )
                .CreateLogger();

            logging.ClearProviders();

            services.AddLogging(builder => builder.AddSerilog(dispose: true));
        }
    }
}
