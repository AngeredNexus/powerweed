using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using WeedDelivery.Backend.Common.Utils;

namespace WeedDelivery.Initialization.Configuration;

public class LogConfiguration
{
    private const string OuterTemplate = "{Timestamp:dd.MM.yy HH:mm:ss} [{Level:u3}][{SourceContext}]: {Message:lj}{NewLine}{Exception}";   
    
    public void SetupGlobalLog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // Request logging // https://github.com/serilog/serilog-aspnetcore
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "pstore.log"),
                rollingInterval: RollingInterval.Day, 
                retainedFileCountLimit: 5, 
                outputTemplate: OuterTemplate, 
                shared: true)
            .WriteTo.File(new LoggerJsonFormatter(),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "pstore.log.json"), 
                rollingInterval: RollingInterval.Day, 
                retainedFileCountLimit: 10,
                shared: true)
            .CreateLogger();

        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
    }
}