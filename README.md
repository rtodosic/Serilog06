## Context
1. [.Net Core Serilog – Basic](https://github.com/rtodosic/Serilog01/)
2. [.Net Core Serilog – Configuration](https://github.com/rtodosic/Serilog02/)
3. [.Net Core Serilog - Structured JSON output](https://github.com/rtodosic/Serilog03/)
4. [.Net Core Serilog - Enrichers](https://github.com/rtodosic/Serilog04/)
5. [.Net Core Serilog - Custom JSON output](https://github.com/rtodosic/Serilog05/)
6. .Net Core Serilog - Adding Sinks

This is part 6 of 6.

## 6. .Net Core Serilog – Adding Sinks

Serilog has quit a lot of [sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks) that can use to write your logs to various places like Seq, ElasticSearch, and Application Insights.
To write to application insights, first [setup application insights in the Azure Portal](https://docs.microsoft.com/en-us/azure/azure-monitor/app/create-new-resource) (you will need the instrumentation key).
1. Add “Serilog.Sinks.ApplicationInsights” NuGet package to your project.
  ![Image alt text](Images/NuGet-Serilog-Sinks-ApplicationInsights.png?raw=true)

2. In Program.cs, add “using Serilog.Templates” to the top of the file and change the  CreateHostBuilder() method to the following (make sure you use your instrumentation key):
  ```C#
  public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          })
      .UseSerilog((hostingContect, loggerConfiguration) => loggerConfiguration
          .MinimumLevel.Warning()
          .MinimumLevel.Override("Serilog06", Serilog.Events.LogEventLevel.Information)
          .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
          .ReadFrom.Configuration(hostingContect.Configuration)
          .Enrich.FromLogContext()
          .Enrich.WithMachineName()
          .Enrich.WithProperty("Assembly", typeof(Program).Assembly.GetName().Name)
          .WriteTo.ApplicationInsights(new TelemetryConfiguration
          {
                InstrumentationKey = "97ebd23e-d23e-d23e-d23e-d23e97ebd23es"
          }, TelemetryConverter.Traces)
          .WriteTo.Console(new ExpressionTemplate("{ {app_timestamp:@t, message:@m, redering:@r, level:if @l = 'Debug' then 'DEBUG' else if @l = 'Warning' then 'WARN' else if @l = 'Error' then 'ERR' else if @l = 'Fatal' then 'FTL' else @l, exception:@x, ..@p} }\n"))
      );
  ```
  
3. Run the application and go into your Azure Application Insights resource. Go into the “logs” section and run the following query:   
  ![Image alt text](Images/Azure-AI-Query.png?raw=true)

[Serilog’s documentation on sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks)
