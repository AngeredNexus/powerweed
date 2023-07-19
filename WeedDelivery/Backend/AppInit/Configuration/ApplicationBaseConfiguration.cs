using Microsoft.AspNetCore.SpaServices.Webpack;
using WeedDelivery.Backend.AppInit.Configuration.Common;
using WeedDelivery.Backend.Models.Configuration.Database;

namespace WeedDelivery.Backend.AppInit.Configuration;

public class ApplicationBaseConfiguration : AppConfiguration
{
    private readonly IConfiguration _appSettings;
    private readonly IConfiguration _appSettingsSecret;

    public ApplicationBaseConfiguration()
    {
        _appSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        _appSettingsSecret = new ConfigurationBuilder()
            .AddJsonFile("appsettings.secret.json")
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
    }

    public override void Configure(IConfiguration configuration)
    {
        
    }
}