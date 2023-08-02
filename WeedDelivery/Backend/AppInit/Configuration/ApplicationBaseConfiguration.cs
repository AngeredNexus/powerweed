using Microsoft.AspNetCore.SpaServices.Webpack;
using WeedDatabase.Models.Configuration.Database;
using WeedDelivery.Backend.AppInit.Configuration.Common;
using WeedDelivery.Backend.Models.Configuration.Bots;

namespace WeedDelivery.Backend.AppInit.Configuration;

public class ApplicationBaseConfiguration : AppConfiguration
{
    private readonly IConfiguration _appSettings;
    private readonly IConfiguration _appSettingsSecret;

    public ApplicationBaseConfiguration()
    {
        _appSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        _appSettingsSecret = new ConfigurationBuilder()
            .AddJsonFile("appsettings.secret.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
    
    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            // https://shwanoff.ru/core-vuejs-1/
            // https://github.com/TrilonIO/aspnetcore-Vue-starter
            
            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
            {
                HotModuleReplacement = true
            });
        }
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppDatabaseConfig>(c => _appSettingsSecret.GetSection("Databases").Bind(c));
        services.Configure<AppTelegramConfiguration>(c => _appSettingsSecret.GetSection("Telegram").Bind(c));
    }

    public override void Configure(IConfiguration configuration)
    {
        
    }
}