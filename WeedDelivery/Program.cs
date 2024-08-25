using Autofac.Extensions.DependencyInjection;
using Serilog;
using WeedDelivery.Initialization;
using WeedDelivery.Initialization.Configuration;
using WeedDelivery.Initialization.Configuration.Kestrel;

var logConfInst = new LogConfiguration();
logConfInst.SetupGlobalLog();

try
{
    Log.Information("Starting web host..");
    CreateWebHostBuilder(args).Build().Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


static IHostBuilder CreateWebHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.UseKestrel(options =>
        {
            options.ConfigureEndpoints(Log.Logger);
            // Set the limit to 128 MB https://bartwullems.blogspot.com/2022/01/aspnet-core-configure-file-upload-size.html#:~:text=By%20default%2C%20ASP.NET%20Core,support%20files%20up%20to%20128MB.&text=The%20code%20above%20configures%20the,the%20MultipartBodyLengthLimit%20property%20to%20128MB.
            // options.Limits.MaxRequestBodySize = 134217728;
        });
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseSerilog();



