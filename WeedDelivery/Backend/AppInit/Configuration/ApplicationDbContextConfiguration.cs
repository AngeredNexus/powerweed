using Microsoft.EntityFrameworkCore;
using WeedDatabase;
using WeedDatabase.Context;
using WeedDatabase.Models.Configuration.Database;
using WeedDelivery.Backend.AppInit.Configuration.Common;

namespace WeedDelivery.Backend.AppInit.Configuration;

public class ApplicationDbContextConfiguration : AppConfiguration
{
    
    private readonly IConfiguration _appSettingsSecret;

    public ApplicationDbContextConfiguration()
    {
        _appSettingsSecret = new ConfigurationBuilder()
            .AddJsonFile("appsettings.secret.json")
            .AddEnvironmentVariables()
            .Build();
        
    }
    
    public override void ConfigureServices(IServiceCollection services)
    {
        var config = _appSettingsSecret.GetSection("Databases").Get<AppDatabaseConfig>();

        if (config is null)
            throw new ConfigurationErrorsException("Database configuration was not found!");
        
        var mainDbConfig = config.Main.ConstructConnectionString();
        var telegramDbConfig = config.Telegram.ConstructConnectionString();

        services.AddDbContext<WeedContext>(
            options => options.UseNpgsql(mainDbConfig));
        
        services.AddDbContext<TelegramContext>(
            options => options.UseNpgsql(telegramDbConfig));
    }
}