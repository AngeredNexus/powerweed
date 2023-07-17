using Autofac.Extensions.DependencyInjection;
using Serilog;
using WeedDelivery.Backend.AppInit;
using WeedDelivery.Backend.AppInit.Configuration;

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
        webBuilder.UseUrls("http://localhost:55525", "https://localhost:55526");
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseSerilog();



