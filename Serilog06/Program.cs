using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Templates;

namespace Serilog06
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Serilog06", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", typeof(Program).Assembly.GetName().Name)
                .WriteTo.ApplicationInsights(new TelemetryConfiguration
                {
                      InstrumentationKey = "97ebd23e-463e-40e8-973a-d395f6575ae3"
                }, TelemetryConverter.Traces)
                .WriteTo.Console(new ExpressionTemplate("{ {app_timestamp:@t, message:@m, redering:@r, level:if @l = 'Debug' then 'DEBUG' else if @l = 'Information' then 'INFO' else if @l = 'Warning' then 'WARN' else if @l = 'Error' then 'ERR' else if @l = 'Fatal' then 'FTL' else @l, exception:@x, ..@p} }\n"))
            );
    }
}
