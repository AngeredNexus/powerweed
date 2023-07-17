using System.Text;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context;
using WeedDatabase.Utils;
using WeedDelivery.Backend.AppInit.Configuration.Common;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Models.Configuration;
using WeedDelivery.Backend.Models.Configuration.Database;

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